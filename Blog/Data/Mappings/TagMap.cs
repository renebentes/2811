using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable(nameof(Tag));

        builder.HasKey(it => it.Id);

        builder.Property(it => it.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.Property(u => u.Slug)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

        builder.HasIndex(u => u.Slug, "IX_Tag_Slug")
            .IsUnique();
    }
}
