namespace UrlShortener.Server.Exceptions;

public class UserAlreadyExistsException() : Exception("User with the same email address or username already exists");
