using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Models;

namespace Todo.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpDelete("/{id:int}")]
    public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.Find(id);

        if (todo == null)
        {
            NotFound();
        }

        context.Todos.Remove(todo);
        context.SaveChanges();

        return NoContent();
    }

    [HttpGet("/")]
    public IActionResult Get([FromServices] AppDbContext context)
        => Ok(context.Todos.ToList());

    [HttpGet("/{id:int}")]
    public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.Find(id);

        if (todo == null)
        {
            NotFound();
        }

        return Ok(todo);
    }

    [HttpPost("/")]
    public IActionResult Post([FromBody] TodoModel todo, [FromServices] AppDbContext context)
    {
        context.Todos.Add(todo);
        context.SaveChanges();

        return CreatedAtRoute($"/{todo.Id}", todo);
    }

    [HttpPut("/{id:int}")]
    public IActionResult Put([FromRoute] int id, [FromBody] TodoModel todo, [FromServices] AppDbContext context)
    {
        var model = context.Todos.Find(id);

        if (model is null)
        {
            return NotFound();
        }

        model.Title = todo.Title;
        model.Done = todo.Done;

        context.Todos.Update(model);
        context.SaveChanges();

        return NoContent();
    }
}
