using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Blog.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger)
        => _logger = logger;

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
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
        catch (DbUpdateException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, "Houve um erro de banco de dados ao remover uma categoria");
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, "Um erro ocorreu ao remover uma categoria");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
    {
        try
        {
            return Ok(await context.Categories.ToListAsync());
        }
        catch (DbException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, "Houve um erro de banco de dados ao obter uma categoria");
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, "Um erro ocorreu ao obter uma categoria");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        catch (DbException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, "Houve um erro de banco de dados ao obter uma categoria");
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, "Um erro ocorreu ao obter uma categoria");
        }
    }

    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] Category model, [FromServices] BlogDataContext context)
    {
        try
        {
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{model.Id}", model);
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, "Houve um erro de banco de dados ao criar uma categoria");
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, "Um erro ocorreu ao criar uma categoria");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] Category model, [FromServices] BlogDataContext context)
    {
        try
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
        catch (DbUpdateException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, "Houve um erro de banco de dados ao atualizar uma categoria");
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, "Um erro ocorreu ao atualizar uma categoria");
        }
    }
}
