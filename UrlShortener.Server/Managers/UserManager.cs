using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Models;
using UrlShortener.Server.Models.Dto;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Managers;

public class UserManager(UserContext userContext, SessionContext sessionContext)
{
    public Roles CheckAccess(string username)
    {
        User user = userContext.Users.FirstOrDefault(u => u.Username == username)
                    ?? throw new NoRoleException();

        return user.Role;
    }

    public SessionDto CreateUser(UserRegisterDto userRegisterDto)
    {
        if (userContext.Users.Any(u => u.Username == userRegisterDto.Username || u.Email == userRegisterDto.Username))
            throw new UserAlreadyExistsException();

        User createdUser = userContext.Users.Add((User)userRegisterDto).Entity;
        userContext.SaveChanges();

        return this.CreateSession(createdUser.Id);
    }

    public SessionDto AuthorizeUser(UserLoginDto userDto)
    {
        User user = userContext.Users.FirstOrDefault(u => u.Username == userDto.Username)
                    ?? throw new AuthorizationFailedException();

        if (user.Password != userDto.Password)
            throw new AuthorizationFailedException();

        return this.CreateSession(user.Id);
    }

    public void AuthorizeSession(SessionDto sessionDto)
    {
        Session session =
                sessionContext.Sessions.FirstOrDefault(s => s.Token == sessionDto.Token &&
                                                             s.UserId == sessionDto.UserId)
                ?? throw new AuthorizationFailedException();

        session.LastAccess = DateTime.Now;

        sessionContext.Sessions.Update(session);
        sessionContext.SaveChanges();
    }

    private SessionDto CreateSession(long userId)
    {
        Session session = new()
        {
                Token = Guid.NewGuid(),
                UserId = userId,
                LastAccess = DateTime.Now
        };

        sessionContext.Sessions.Add(session);
        sessionContext.SaveChanges();

        return (SessionDto)session;
    }
}
