using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Contexts;

public class ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options) : DbContext(options)
{
    private DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public ShortenedUrl FindById(long id) => this.ShortenedUrls.Find(id);

    public ShortenedUrl FindIncludedById(long id) =>
            this.ShortenedUrls.Include(su => su.Author).FirstOrDefault(su => su.Id == id);

    public ShortenedUrl FindByCode(string code) =>
            this.ShortenedUrls.Include(su => su.Author).FirstOrDefault(su => su.Code == code);

    public List<ShortenedUrl> GetAll() => this.ShortenedUrls.ToList();

    public bool HasByCode(string code) => this.ShortenedUrls.Any(su => su.Code == code);
    
    public bool HasByFullUrl(string fullUrl) => this.ShortenedUrls.Any(su => su.FullUrl == fullUrl);
}
