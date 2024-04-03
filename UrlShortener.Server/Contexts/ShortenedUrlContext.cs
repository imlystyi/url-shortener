using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Contexts;

public class ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public bool HasShortUrl(string shortUrl) => this.ShortenedUrls.Any(su => su.ShortUrl == shortUrl);
}
