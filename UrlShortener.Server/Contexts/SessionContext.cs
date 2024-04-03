using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Contexts;

public class SessionContext(DbContextOptions<SessionContext> options) : DbContext(options)
{
    public DbSet<Session> Sessions { get; set; }
}
