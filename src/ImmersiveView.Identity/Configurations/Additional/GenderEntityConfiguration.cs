using ImmersiveView.Identity.Entities.Additional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImmersiveView.Identity.Configurations.Additional;

public class GenderEntityConfiguration : IEntityTypeConfiguration<Gender>
{
    public void Configure(EntityTypeBuilder<Gender> builder)
    {
        builder.Property(propertyName => propertyName.GenderName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasMany(propertyName => propertyName.ApplicationUsers)
            .WithOne(propertyName => propertyName.Gender)
            .HasForeignKey(propertyName => propertyName.GenderId)
            .IsRequired(false);
    }
}