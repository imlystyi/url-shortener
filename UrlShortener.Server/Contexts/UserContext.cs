using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Contexts;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
