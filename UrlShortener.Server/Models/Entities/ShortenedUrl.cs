using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Dto;

namespace UrlShortener.Server.Models.Entities;

[Table("shortened_url")]
[Index(nameof(Code), IsUnique = true)]
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

    [Column("code")]
    [StringLength(32)]
    [Required]
    public string Code { get; set; }

    [Column("clicks")]
    [Required]
    public long Clicks { get; set; }

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }

    public User Author { get; set; }
}
