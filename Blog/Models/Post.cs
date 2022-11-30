namespace Blog.Models;

public class Post : ModelBase
{
    public User Author { get; set; } = new();

    public int AuthorId { get; set; }

    public string Body { get; set; } = string.Empty;

    public Category Category { get; set; } = new();

    public int CategoryId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public IList<Tag> Tags { get; set; } = new List<Tag>();

    public string Title { get; set; } = string.Empty;
}
