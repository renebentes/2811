using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IEnumerable<TodoModel> Get([FromServices] AppDbContext context)
        => context.Todos.ToList();
}
