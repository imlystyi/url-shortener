namespace UrlShortener.Server.Models.Dto;

public class ShortenedUrlInfoOutputDto
{
    public long Id { get; set; }

    public string AuthorNickname { get; set; }

    public string FullUrl { get; set; }

    public string ShortUrl { get; set; }

    public long Clicks { get; set; }

    public string CreatedAt { get; set; }
}
