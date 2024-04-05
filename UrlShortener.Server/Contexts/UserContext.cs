using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Contexts;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public User FindById(long id) => this.Users.Find(id);
    public User FindByUsername(string username) => this.Users.FirstOrDefault(u => u.Username == username);

    public bool HasByUsernameAndEmail(string username, string email) => this.Users.Any(u => u.Username == username || u.Email == email);
}
