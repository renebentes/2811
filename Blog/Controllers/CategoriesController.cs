using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context, [FromServices] IMemoryCache cache)
    {
        try
        {
            var categories = await cache.GetOrCreateAsync("CategoriesCache", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return await context.Categories.ToListAsync();
            });

            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch (DbException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, new ResultViewModel<List<Category>>("Houve um erro de banco de dados ao obter uma categoria"));
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, new ResultViewModel<List<Category>>("Um erro ocorreu ao obter uma categoria"));
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

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Houve um erro de banco de dados ao obter uma categoria"));
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Um erro ocorreu ao obter uma categoria"));
        }
    }

    [HttpPost()]
    public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = new Category
            {
                Title = model.Title,
                Slug = model.Slug
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Houve um erro de banco de dados ao criar uma categoria"));
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Um erro ocorreu ao criar uma categoria"));
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] EditorCategoryViewModel model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return NotFound(new ResultViewModel<Category>("Nenhuma categoria encontrada"));
            }

            category.Title = model.Title;
            category.Slug = model.Slug;

            context.Categories.Update(category);

            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException e)
        {
            _logger.LogCritical(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Houve um erro de banco de dados ao atualizar uma categoria"));
        }
        catch (Exception e)
        {
            _logger.LogError(message: e.ToString());
            return StatusCode(500, new ResultViewModel<Category>("Um erro ocorreu ao atualizar uma categoria"));
        }
    }
}
