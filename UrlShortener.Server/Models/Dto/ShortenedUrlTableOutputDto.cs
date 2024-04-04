using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlTableOutputDto
{
    public long Id { get; set; }

    public string FullUrl { get; set; }

    public string ShortUrl { get; set; }

    public static explicit operator ShortenedUrlTableOutputDto(ShortenedUrl v) => new()
    {
            Id = v.Id,
            FullUrl = v.FullUrl,
            ShortUrl = "https://localhost:7190/" + v.Code
    };
}
