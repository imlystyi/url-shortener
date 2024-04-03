using System.ComponentModel.DataAnnotations;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlCreateDto
{
    public required long AuthorId { get; set; }

    [StringLength(2048)]
    public required string FullUrl { get; set; }

    [StringLength(32)]
    public required string ShortUrl { get; set; }

    public static explicit operator ShortenedUrl(ShortenedUrlCreateDto v) => new()
    {
            AuthorId = v.AuthorId,
            FullUrl = v.FullUrl,
            ShortUrl = v.ShortUrl,
            Clicks = 0,
            CreatedAt = DateTime.Now
    };
}

