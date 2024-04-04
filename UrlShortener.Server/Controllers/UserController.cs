using Microsoft.AspNetCore.Mvc;
using UrlShortener.Server.Contexts;
using UrlShortener.Server.Exceptions;
using UrlShortener.Server.Managers;
using UrlShortener.Server.Models;
using UrlShortener.Server.Models.Dto;

namespace UrlShortener.Server.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(UserContext userContext, SessionContext sessionContext) : Controller
{
    private readonly UserManager _userManager = new(userContext, sessionContext);

    [HttpGet("check-access/{id}")]
    public ActionResult<Roles> CheckAccess([FromRoute] long id)
    {
        try
        {
            Roles role = _userManager.CheckAccess(id);

            return this.Ok(role);
        }
        catch (NoRoleException)
        {
            return this.Ok(null);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("register")]
    public ActionResult<SessionDto> Register([FromBody] UserRegisterDto userDto)
    {
        try
        {
            SessionDto sessionDto = _userManager.CreateUser(userDto);

            return this.Ok(sessionDto);
        }
        catch (UserAlreadyExistsException)
        {
            return this.Conflict();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("login")]
    public ActionResult<SessionDto> LoginByUserIdentities([FromBody] UserLoginDto userDto)
    {
        try
        {
            SessionDto sessionDto = _userManager.AuthorizeUser(userDto);

            return this.Ok(sessionDto);
        }
        catch (AuthorizationFailedException)
        {
            return this.Unauthorized();
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("login/session")]
    public ActionResult LoginBySessionIdentities([FromBody] SessionDto sessionDto)
    {
        try
        {
            _userManager.AuthorizeSession(sessionDto);

            return this.Ok();
        }
        catch (AuthorizationFailedException)
        {
            return new StatusCodeResult(StatusCodes.Status100Continue);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
