using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(nameof(Post));

        builder.HasKey(it => it.Id);

        builder.Property(it => it.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(it => it.Title)
           .IsRequired()
           .HasColumnType("VARCHAR")
           .HasMaxLength(160);

        builder.Property(it => it.Summary)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(255);

        builder.Property(it => it.Body)
            .IsRequired()
            .HasColumnType("TEXT");

        builder.Property(it => it.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(it => it.CreateDate)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");

        // Para definir valor default via .NET
        //.HasDefaultValue(DateTime.Now.ToUniversalTime())

        builder.Property(it => it.LastUpdateDate)
            .IsRequired()
            .HasColumnType("SMALLDATETIME")
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(it => it.Slug, "IX_Post_Slug")
            .IsUnique();

        // Relacionamentos
        builder.HasOne(it => it.Author)
            .WithMany(it => it.Posts)
            .HasConstraintName("FK_Post_Author")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(it => it.Category)
            .WithMany(it => it.Posts)
            .HasConstraintName("FK_Post_Category")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(it => it.Tags)
            .WithMany(it => it.Posts)
            .UsingEntity<Dictionary<string, object>>(
                "PostTag",
                tag => tag.HasOne<Tag>()
                            .WithMany()
                            .HasForeignKey("TagId")
                            .HasConstraintName("FK_PostTag_TagId")
                            .OnDelete(DeleteBehavior.Cascade),
                post => post.HasOne<Post>()
                          .WithMany()
                          .HasForeignKey("PostId")
                          .HasConstraintName("FK_PostTag_PostId")
                          .OnDelete(DeleteBehavior.Cascade));
    }
}
