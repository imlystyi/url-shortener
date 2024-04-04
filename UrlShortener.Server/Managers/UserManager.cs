using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Models;
using UrlShortener.Server.Models.Dto;
using UrlShortener.Server.Models.Entities;

namespace UrlShortener.Server.Managers;

public class UserManager(UserContext userContext, SessionContext sessionContext)
{
    #region Fields

    private readonly UserContext _userContext = userContext;
    private readonly SessionContext _sessionContext = sessionContext;

    #endregion

    #region Methods

    public Roles CheckAccess(string username)
    {
        User user = _userContext.Users.FirstOrDefault(u => u.Username == username)
                    ?? throw new NoRoleException();

        return user.Role;
    }

    public SessionDto CreateUser(UserRegisterDto userRegisterDto)
    {
        if (_userContext.Users.Any(u => u.Username == userRegisterDto.Username || u.Email == userRegisterDto.Username))
            throw new UserAlreadyExistsException();

        User createdUser = _userContext.Users.Add((User)userRegisterDto).Entity;
        _userContext.SaveChanges();

        return this.CreateSession(createdUser.Id);
    }

    public SessionDto AuthorizeUser(UserLoginDto userDto)
    {
        User user = _userContext.Users.FirstOrDefault(u => u.Username == userDto.Username)
                    ?? throw new AuthorizationFailedException();

        if (user.Password != userDto.Password)
            throw new AuthorizationFailedException();

        return this.CreateSession(user.Id);
    }

    public void AuthorizeSession(SessionDto sessionDto)
    {
        Session session =
                _sessionContext.Sessions.FirstOrDefault(s => s.Token == sessionDto.Token &&
                                                             s.UserId == sessionDto.UserId)
                ?? throw new AuthorizationFailedException();

        session.LastAccess = DateTime.Now;

        _sessionContext.Sessions.Update(session);
        _sessionContext.SaveChanges();
    }

    private SessionDto CreateSession(long userId)
    {
        Session session = new()
        {
                Token = Guid.NewGuid(),
                UserId = userId,
                LastAccess = DateTime.Now
        };

        _sessionContext.Sessions.Add(session);
        _sessionContext.SaveChanges();

        return (SessionDto)session;
    }

    #endregion
}
