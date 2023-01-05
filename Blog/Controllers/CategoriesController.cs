using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        var category = await context.Categories.SingleOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        context.Categories.Remove(category);

        await context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        => Ok(await context.Categories.ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        var category = await context.Categories.SingleOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] Category model, [FromServices] BlogDataContext context)
    {
        await context.Categories.AddAsync(model);
        await context.SaveChangesAsync();

        return Created($"v1/categories/{model.Id}", model);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model, [FromServices] BlogDataContext context)
    {
        var category = await context.Categories.SingleOrDefaultAsync(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        category.Title = model.Title;
        category.Slug = model.Slug;

        context.Categories.Update(category);

        await context.SaveChangesAsync();

        return Ok(category);
    }
}
