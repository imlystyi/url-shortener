using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Models;
using UrlShortener.Server.Models.Dto;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Managers;

public class UserManager(UserContext userContext, SessionContext sessionContext)
{
    public Roles CheckAccess(long id)
    {
        User user = userContext.FindById(id) ?? throw new NoRoleException();

        return user.Role;
    }

    public SessionDto CreateUser(UserRegisterDto registerDto)
    {
        if (userContext.HasByUsernameAndEmail(registerDto.Username, registerDto.Email))
            throw new UserAlreadyExistsException();

        User createdUser = userContext.Add((User)registerDto).Entity;
        userContext.SaveChanges();

        return this.CreateSession(createdUser.Id);
    }

    public SessionDto AuthorizeUser(UserLoginDto loginDto)
    {
        User user = userContext.FindByUsername(loginDto.Username)
                    ?? throw new AuthorizationFailedException();

        if (user.Password != loginDto.Password)
            throw new AuthorizationFailedException();

        return this.CreateSession(user.Id);
    }

    public void AuthorizeSession(SessionDto sessionDto)
    {
        Session session =
                sessionContext.FindByTokenAndUserId(sessionDto.Token, sessionDto.UserId)
                ?? throw new AuthorizationFailedException();

        session.LastAccess = DateTime.Now;

        sessionContext.Update(session);
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

        sessionContext.Add(session);
        sessionContext.SaveChanges();

        return (SessionDto)session;
    }
}
