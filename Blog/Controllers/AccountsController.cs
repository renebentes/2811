using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AccountController(TokenService tokenService)
        => _tokenService = tokenService;

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
    public IActionResult SignIn()
        => Ok(_tokenService.GenerateToken(null));
}
