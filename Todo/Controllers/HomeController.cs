using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpDelete("/{id:int}")]
    public TodoModel Delete([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.Find(id);

        context.Todos.Remove(todo);
        context.SaveChanges();

        return todo;
    }

    [HttpGet("/")]
    public IEnumerable<TodoModel> Get([FromServices] AppDbContext context)
        => context.Todos.ToList();

    [HttpGet("/{id:int}")]
    public TodoModel GetById([FromRoute] int id, [FromServices] AppDbContext context)
        => context.Todos.FirstOrDefault(todo => todo.Id == id);

    [HttpPost("/")]
    public TodoModel Post([FromBody] TodoModel todo, [FromServices] AppDbContext context)
    {
        context.Todos.Add(todo);
        context.SaveChanges();

        return todo;
    }

    [HttpPut("/{id:int}")]
    public TodoModel Put([FromRoute] int id, [FromBody] TodoModel todo, [FromServices] AppDbContext context)
    {
        var model = context.Todos.Find(id);

        if (model is null)
        {
            return todo;
        }

        model.Title = todo.Title;
        model.Done = todo.Done;

        context.Todos.Update(model);
        context.SaveChanges();

        return todo;
    }
}
