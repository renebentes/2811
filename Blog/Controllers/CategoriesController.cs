using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] BlogDataContext context)
    {
        return Ok(context.Categories.ToList());
    }
}
