using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Models.Dto;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Managers;

public class ShortenedUrlManager(ShortenedUrlContext shortenedUrlContext)
{
    #region Fields

    private const int _ShortenUrlLength = 5,
                      _MaxAttempts = 100;

    private const string _Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    #endregion

    #region Methods

    public IEnumerable<ShortenedUrlTableOutputDto> GetAllShortenedUrl()
    {
        return shortenedUrlContext.GetAll().Select(su => (ShortenedUrlTableOutputDto)su);
    }

    public ShortenedUrlInfoOutputDto GetShortenedUrlInfo(long id)
    {
        ShortenedUrl shortenedUrl = shortenedUrlContext.FindIncludedById(id) ?? throw new ShortenedUrlNotFound();

        return new()
        {
                AuthorNickname = shortenedUrl.Author.Username,
                FullUrl = shortenedUrl.FullUrl,
                ShortUrl = "https://localhost:7190/" + shortenedUrl.Code,
                Clicks = shortenedUrl.Clicks,
                CreatedAt = shortenedUrl.CreatedAt.ToString("g")
        };
    }

    public string GetFullUrl(string code)
    {
        ShortenedUrl shortenedUrl = shortenedUrlContext.FindByCode(code) ?? throw new ShortenedUrlNotFound();

        shortenedUrl.Clicks++;
        shortenedUrlContext.SaveChanges();

        return shortenedUrl.FullUrl;
    }

    public void AddShortenedUrl(ShortenedUrlInputDto inputDto)
    {
        if (shortenedUrlContext.HasByFullUrl(inputDto.FullUrl))
            throw new FullUrlAlreadyShortenedException();

        for (int i = 0; i <= _MaxAttempts; i++)
        {
            string code = this.GenerateRandomCode();

            if (shortenedUrlContext.HasByCode(code))
                continue;

            ShortenedUrl shortenedUrl = new()
            {
                    FullUrl = inputDto.FullUrl,
                    Code = code,
                    AuthorId = inputDto.AuthorId,
                    Clicks = 0,
                    CreatedAt = DateTime.Now
            };

            shortenedUrlContext.Add(shortenedUrl);
            shortenedUrlContext.SaveChanges();

            return;
        }

        throw new MaxAttemptsReachedException();
    }

    public void DeleteShortenedUrl(long id)
    {
        shortenedUrlContext.Remove
                (shortenedUrlContext.FindById(id) ?? throw new ShortenedUrlNotFound());
        shortenedUrlContext.SaveChanges();
    }

    public void DeleteAllShortenedUrls()
    {
        shortenedUrlContext.Database.ExecuteSqlRaw("DELETE FROM shortened_url");
        shortenedUrlContext.SaveChanges();
    }

    private string GenerateRandomCode()
    {
        Random random = new();
        StringBuilder sb = new();

        Parallel.For(0,
                     _ShortenUrlLength,
                     _ => sb.Append(_Alphabet[random.Next(_Alphabet.Length - 1)]));

        return sb.ToString();
    }

    #endregion
}
