using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlUpdateDto
{
    public required long Id { get; set; }

    [StringLength(2048)]
    public required string FullUrl { get; set; }

    [StringLength(32)]
    public required string ShortUrl { get; set; }
}
