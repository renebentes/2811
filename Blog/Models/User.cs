namespace Blog.Models;

public class User : ModelBase
{
    public string Bio { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public IList<Post> Posts { get; set; } = new List<Post>();

    public IList<Role> Roles { get; set; } = new List<Role>();

    public string Slug { get; set; } = string.Empty;
}
