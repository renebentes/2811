namespace Todo.Models;

public class Todo
{
    public DateTime CreatedAt { get; set; }

    public bool Done { get; set; }

    public int Id { get; set; }

    public string Title { get; set; }
}