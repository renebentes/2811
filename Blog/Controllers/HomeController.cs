using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult HealthCheck()
        => Ok();
}
