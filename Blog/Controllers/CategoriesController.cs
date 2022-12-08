using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        => Ok(await context.Categories.ToListAsync());
}
