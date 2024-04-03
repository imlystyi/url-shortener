using System.Text;
using UrlShortener.Server.Contexts;
using UrlShortener.Server.Models.Dto;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Managers;

public class ShortenedUrlManager(ShortenedUrlContext shortenedUrlContext)
{
    #region Fields

    private const int _ShortenUrlLength = 5,
                      _MaxAttempts = 100;

    private const string _Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                         _ShortUrlPrefix = "https://localhost:5001/";

    private readonly ShortenedUrlContext _shortenedUrlContext = shortenedUrlContext;

    #endregion

    #region Methods

    public ShortenedUrlCreateDto ShortenUrl(string fullUrl, long authorId)
    {
        if (_shortenedUrlContext.ShortenedUrls.Any(su => su.FullUrl == fullUrl))
            throw new InvalidOperationException("This URL has already been shortened."); // todo: custom exception

        for (int i = 0; i <= _MaxAttempts; i++)
        {
            string shortUrl = _ShortUrlPrefix + this.GenerateRandomCode();

            if (!_shortenedUrlContext.HasShortUrl(shortUrl))
                return new()
                {
                        AuthorId = authorId,
                        FullUrl = fullUrl,
                        ShortUrl = shortUrl
                };
        }

        throw new InvalidOperationException
                ("Failed to generate a unique short URL. Try again later."); // todo: custom exception
    }

    public void AddShortenedUrl(ShortenedUrlCreateDto shortenedUrlDto)
    {
        _shortenedUrlContext.ShortenedUrls.Add((ShortenedUrl)shortenedUrlDto);
        _shortenedUrlContext.SaveChanges();
    }

    public void UpdateShortenedUrl(ShortenedUrlUpdateDto shortenedUrlDto)
    {
        ShortenedUrl shortenedUrl = _shortenedUrlContext.ShortenedUrls.Find(shortenedUrlDto.Id)
                                    ?? throw new
                                            InvalidOperationException("Shortened URL not found."); // todo: custom exception

        shortenedUrl.FullUrl = shortenedUrlDto.FullUrl;
        shortenedUrl.ShortUrl = shortenedUrlDto.ShortUrl;

        _shortenedUrlContext.ShortenedUrls.Update(shortenedUrl);
        _shortenedUrlContext.SaveChanges();
    }

    public void DeleteShortenedUrl(long id)
    {
        _shortenedUrlContext.ShortenedUrls.Remove
                (_shortenedUrlContext.ShortenedUrls.Find(id) ??
                 throw new InvalidOperationException("Shortened URL not found.")); // todo: custom exception
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
