using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] BlogDataContext context)
    {
        return Ok(context.Categories.ToList());
    }
}
