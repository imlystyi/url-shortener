using System.ComponentModel.DataAnnotations;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlInputDto
{
    public required long AuthorId { get; set; }

    [StringLength(2048)]
    public required string FullUrl { get; set; }

    [StringLength(32)]
    public required string ShortUrl { get; set; }

    public static explicit operator ShortenedUrl(ShortenedUrlInputDto shortenedUrlDto) => new()
    {
            AuthorId = shortenedUrlDto.AuthorId,
            FullUrl = shortenedUrlDto.FullUrl,
            ShortUrl = shortenedUrlDto.ShortUrl,
            Clicks = 0,
            CreatedAt = DateTime.Now
    };
}

