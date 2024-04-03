using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Server.Models.Entities;

[Table("user")]
public class User
{
    [Column("id")]
    [Key]
    public long Id { get; set; }

    [Column("role")]
    public Roles Role { get; set; }

    [Column("username")]
    [StringLength(32)]
    public string Username { get; set; }

    [Column("password")]
    [StringLength(64)]
    public string Password { get; set; }

    [Column("email")]
    [StringLength(320)]
    [EmailAddress]
    public string Email { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
