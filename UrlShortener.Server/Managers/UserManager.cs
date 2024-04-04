﻿using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
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

    public SessionDto CreateUser(UserCreateDto userCreateDto)
    {
        if (_userContext.Users.Any(u => u.Username == userCreateDto.Username || u.Email == userCreateDto.Username))
            throw new InvalidOperationException(); // todo: custom exception

        User createdUser = _userContext.Users.Add((User)userCreateDto).Entity;
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
        Session session = _sessionContext.Sessions.FirstOrDefault
                (s => s.Token == sessionDto.Token && s.UserId == sessionDto.UserId)
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
