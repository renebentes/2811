using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    public async Task<IActionResult> ListPosts([FromServices] BlogDataContext context)
    {
        var posts = await context
            .Posts
            .AsNoTracking()
            .Include(p=>p.Category)
            .Include(p=>p.Author)
            .Select(p=> new ListPostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Slug = p.Slug,
                LastUpdateDate = p.LastUpdateDate,
                Category = p.Category.Title,
                Author = $"{p.Author.Name} ({p.Author.Email})"
            })
            .ToListAsync();

        return Ok(posts);
    }
}
