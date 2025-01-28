using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pipeline.Empresa.Constants;
using Pipeline.Empresa.Entities;

namespace Pipeline.Empresa.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(ILogger<UserController> logger) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        return Ok($"Hello World {User.Identity?.Name}!");
    }

    [HttpGet("users")]
    [Authorize]
    public ActionResult<ICollection<User>> GetUsers(CancellationToken cancellation = default)
    {
        return Ok(new
        {
            Message = "Hello World!"
        });
    }

    [HttpGet("users/{idUser:int}")]
    [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Client},{RoleConstants.Employee}")]
    public ActionResult<User> GetUser(Int32 idUser, CancellationToken cancellation = default)
    {
        return Ok(new
        {
            Message = "Hello World!"
        });
    }
}

