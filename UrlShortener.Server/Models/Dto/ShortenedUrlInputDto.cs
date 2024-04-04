using System.ComponentModel.DataAnnotations;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlInputDto
{
    public required long AuthorId { get; set; }

    [StringLength(2048)]
    public required string FullUrl { get; set; }

    public static implicit operator ShortenedUrl(ShortenedUrlInputDto v) => new()
    {
            AuthorId = v.AuthorId,
            FullUrl = v.FullUrl,
            Clicks = 0,
            CreatedAt = DateTime.Now
    };
}

