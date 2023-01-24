using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));

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

        builder.HasIndex(u => u.Slug, "IX_Role_Slug")
            .IsUnique();
    }
}
