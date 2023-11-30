using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Infrastructure.Configuration;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasIndex(species => species.Name)
            .IsUnique();

        builder.Property(species => species.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Species.NameMaxLength);

        builder.Property(species => species.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.HasData(
            new { Id = 1, Name = "Dog", CreatedBy = Guid.Empty },
            new { Id = 2, Name = "Cat", CreatedBy = Guid.Empty },
            new { Id = 3, Name = "Horse", CreatedBy = Guid.Empty });
    }
}