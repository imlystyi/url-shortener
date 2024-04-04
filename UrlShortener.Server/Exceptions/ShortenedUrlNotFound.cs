namespace UrlShortener.Server.Exceptions;

public class ShortenedUrlNotFound() : Exception("There is no shortened URL with such id");
