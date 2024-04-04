using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Contexts;

namespace UrlShortener.Server;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<UserContext>
                (options => options.UseSqlServer(builder.Configuration
                                                        .GetConnectionString("Default")));
        builder.Services.AddDbContext<ShortenedUrlContext>
                (options => options.UseSqlServer(builder.Configuration
                                                        .GetConnectionString("Default")));
        builder.Services.AddDbContext<SessionContext>
                (options => options.UseSqlServer(builder.Configuration
                                                        .GetConnectionString("Default")));

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                corsBuilder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
            });
        });

        WebApplication app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.UseCors();

        app.Run();
    }
}
