using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Server.Contexts;
using UrlShortener.Server.Controllers;
using UrlShortener.Server.Models;
using UrlShortener.Server.Models.Dto;

namespace UrlShortener.Server.Tests.ControllerTests;

public class UserControllerTests
{
    [Test]
    public void CheckAccessTest()
    {
        SessionContext sessionContext = new
                (new DbContextOptionsBuilder<SessionContext>().UseInMemoryDatabase("UserControllerTestsCheckAccessTest")
                                                              .Options);
        UserContext userContext = new
                (new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase("UserControllerTestsCheckAccessTest")
                                                           .Options);
        UserController userController = new(userContext, sessionContext);

        UserRegisterDto userRegisterDto1 = new()
        {
                Role = Roles.Admin,
                Username = "test1",
                Email = "test1@test.com",
                Password = "test1password"
        };

        _ = userController.Register(userRegisterDto1);
        ActionResult<Roles> actionResult2 = userController.CheckAccess(userRegisterDto1.Username);
        ActionResult<Roles> actionResult3 = userController.CheckAccess("invalid");
        Assert.Multiple(() =>
        {
            Assert.That(actionResult2.Result, Is.TypeOf<OkObjectResult>());
            Assert.That((actionResult2.Result as OkObjectResult)?.Value, Is.EqualTo(Roles.Admin));
            Assert.That(actionResult3.Result, Is.TypeOf<OkObjectResult>());
            Assert.That((actionResult3.Result as OkObjectResult)?.Value, Is.Null);
        });
    }

    [Test]
    public void RegisterTest()
    {
        SessionContext sessionContext = new
                (new DbContextOptionsBuilder<SessionContext>().UseInMemoryDatabase("UserControllerTestsRegisterTest")
                                                              .Options);
        UserContext userContext = new
                (new DbContextOptionsBuilder<UserContext>().UseInMemoryDatabase("UserControllerTestsRegisterTest")
                                                           .Options);
        UserController userController = new(userContext, sessionContext);

        UserRegisterDto userRegisterDto1 = new()
        {
                Role = Roles.Admin,
                Username = "test1",
                Email = "test1@test.com",
                Password = "test1password"
        };
        UserRegisterDto userRegisterDto2 = new()
        {
                Role = Roles.User,
                Username = "test1",
                Email = "test3@email.com",
                Password = "test3password"
        };

        ActionResult<SessionDto> actionResult1 = userController.Register(userRegisterDto1);
        Assert.That(actionResult1.Result, Is.TypeOf<OkObjectResult>());

        ActionResult<SessionDto> actionResult2 = userController.Register(userRegisterDto2);
        Assert.That(actionResult2.Result, Is.TypeOf<ConflictResult>());
    }

    [Test]
    public void LoginByUserIdentitiesTest()
    {
        SessionContext sessionContext = new
                (new DbContextOptionsBuilder<SessionContext>()
                .UseInMemoryDatabase("UserControllerTestsLoginByUserIdentitiesTest").Options);
        UserContext userContext = new
                (new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("UserControllerTestsLoginByUserIdentitiesTest").Options);
        UserController userController = new(userContext, sessionContext);

        UserRegisterDto userRegisterDto1 = new()
        {
                Role = Roles.Admin,
                Username = "test1",
                Email = "test1@test.com",
                Password = "test1password"
        };
        UserLoginDto userLoginDto1 = new()
        {
                Username = userRegisterDto1.Username,
                Password = userRegisterDto1.Password
        };

        ActionResult<SessionDto> actionResult = userController.LoginByUserIdentities(userLoginDto1);
        Assert.That(actionResult.Result, Is.TypeOf<UnauthorizedResult>());

        userController.Register(userRegisterDto1);
        ActionResult<SessionDto> actionResult2 = userController.LoginByUserIdentities(userLoginDto1);
        Assert.That(actionResult2.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public void LoginBySessionIdentitiesTest()
    {
        SessionContext sessionContext = new
                (new DbContextOptionsBuilder<SessionContext>()
                .UseInMemoryDatabase("UserControllerTestsLoginBySessionIdentitiesTest").Options);
        UserContext userContext = new
                (new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("UserControllerTestsLoginBySessionIdentitiesTest").Options);
        UserController userController = new(userContext, sessionContext);

        UserRegisterDto userRegisterDto1 = new()
        {
                Role = Roles.Admin,
                Username = "test1",
                Email = "test1@test.com",
                Password = "test1password"
        };
        SessionDto sessionDto = new()
        {
                Token = Guid.NewGuid(),
                UserId = 1
        };

        ActionResult actionResult1 = userController.LoginBySessionIdentities(sessionDto);
        Assert.Multiple(() =>
        {
            Assert.That(actionResult1, Is.TypeOf<StatusCodeResult>());
            Assert.That((actionResult1 as StatusCodeResult)?.StatusCode, Is.EqualTo(100));
        });

        ActionResult<SessionDto> actionResult2 = userController.Register(userRegisterDto1);
        SessionDto sessionDto2 = (actionResult2.Result as OkObjectResult)?.Value as SessionDto;
        ActionResult actionResult3 = userController.LoginBySessionIdentities(sessionDto2);
        Assert.That(actionResult3, Is.TypeOf<OkResult>());
    }
}
