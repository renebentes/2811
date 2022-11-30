namespace Blog.Models;

public class Tag : ModelBase
{
    public string Name { get; set; } = string.Empty;

    public IList<Post> Posts { get; set; } = new List<Post>();

    public string Slug { get; set; } = string.Empty;
}
