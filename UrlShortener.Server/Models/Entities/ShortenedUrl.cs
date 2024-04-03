using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Server.Models.Entities;

[Table("shortened_url")]
[Index(nameof(ShortUrl), IsUnique = true)]
public class ShortenedUrl
{
    [Column("id")]
    [Key]
    public long Id { get; set; }

    [Column("author_id")]
    [ForeignKey("author")]
    [Required]
    public long AuthorId { get; set; }

    [Column("full_url")]
    [StringLength(2048)]
    [Required]
    public string FullUrl { get; set; }

    [Column("short_url")]
    [StringLength(32)]
    [Required]
    public string ShortUrl { get; set; }

    [Column("clicks")]
    [Required]
    public long Clicks { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }

    public User Author { get; set; }
}
