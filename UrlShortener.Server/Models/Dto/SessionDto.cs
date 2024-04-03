using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Models.Dto;

public class SessionDto
{
    public Guid Token { get; set; }

    public long UserId { get; set; }

    public static explicit operator SessionDto(Session v) => new()
    {
        Token = v.Token,
        UserId = v.UserId
    };
}
