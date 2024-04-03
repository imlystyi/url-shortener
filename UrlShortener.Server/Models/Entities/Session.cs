using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Server.Models.Entities;

[Table("session")]
[Index(nameof(Token), IsUnique = true)]
public class Session
{
    [Column("id")]
    [Key]
    public long Id { get; set; }

    [Column("token")]
    [Required]
    public Guid Token { get; set; }

    [Column("user_id")]
    [ForeignKey("user")]
    [Required]
    public long UserId { get; set; }

    [Required]
    public DateTime LastAccess { get; set; }

    public User User { get; set; }
}
