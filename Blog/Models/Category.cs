namespace Blog.Models;

public class Category : ModelBase
{
    public IList<Post> Posts { get; set; } = new List<Post>();

    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
}
