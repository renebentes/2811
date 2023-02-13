using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
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
}
