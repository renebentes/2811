using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListPosts([FromServices] BlogDataContext context,
                                               [FromQuery] int page = 0,
                                               [FromQuery] int pageSize = 25)
    {
        try
        {
            var total = await context
                .Posts
                .AsNoTracking()
                .CountAsync();

            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Select(p => new ListPostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    LastUpdateDate = p.LastUpdateDate,
                    Category = p.Category.Title,
                    Author = $"{p.Author.Name} ({p.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(p => p.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total,
                page,
                pageSize,
                posts
            }));
        }
        catch (Exception e)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor."));
        }
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> ListPostsByCategory([FromServices] BlogDataContext context,
                                                         [FromRoute] string category,
                                                         [FromQuery] int page = 0,
                                                         [FromQuery] int pageSize = 25)
    {
        try
        {
            var total = await context
                .Posts
                .AsNoTracking()
                .CountAsync();

            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Author)
                .Where(p => p.Slug == category)
                .Select(p => new ListPostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    LastUpdateDate = p.LastUpdateDate,
                    Category = p.Category.Title,
                    Author = $"{p.Author.Name} ({p.Author.Email})"
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(p => p.LastUpdateDate)
                .ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                total,
                page,
                pageSize,
                posts
            }));
        }
        catch (Exception e)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor."));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ShowPost([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var post = await context
                .Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .ThenInclude(u => u.Roles)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post is null)
            {
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));
            }

            return Ok(new ResultViewModel<Post>(post));
        }
        catch (Exception e)
        {
            // TODO: Log exception
            return StatusCode(500, new ResultViewModel<List<Post>>("Erro interno do servidor."));
        }
    }
}
