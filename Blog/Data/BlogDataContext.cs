using Blog.Data.Mappings;

namespace Blog.Data;

public class BlogDataContext : DbContext
{
    public BlogDataContext(DbContextOptions<BlogDataContext> options)
        : base(options)
    { }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new PostMap());
        modelBuilder.ApplyConfiguration(new RoleMap());
        modelBuilder.ApplyConfiguration(new TagMap());
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}
