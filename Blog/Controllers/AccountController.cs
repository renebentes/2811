using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AccountController(TokenService tokenService)
        => _tokenService = tokenService;

    [HttpPost("signin")]
    public IActionResult SigIn(TokenService tokenService)
        => Ok(tokenService.GenerateToken(null));
}
