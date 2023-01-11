using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    [Authorize("admin")]
    public IActionResult GetAdmin()
        => Ok(User?.Identity?.Name);

    [Authorize("author")]
    public IActionResult GetAuthor()
        => Ok(User?.Identity?.Name);

    [Authorize("user")]
    public IActionResult GetUser()
        => Ok(User?.Identity?.Name);

    [HttpPost("signin")]
    public IActionResult SignIn([FromServices] TokenService tokenService)
        => Ok(tokenService.GenerateToken(null));
}
