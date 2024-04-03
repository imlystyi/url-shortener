namespace UrlShortener.Server.Models.Dto
{
    public class UserLoginDto
    {
        public string Username { get; internal set; }
        public string Password { get; set; }
    }
}