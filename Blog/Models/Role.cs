namespace Blog.Models;

public class Role : ModelBase
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public IList<User> Users { get; set; } = new List<User>();
}
