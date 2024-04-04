using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Database.Configuration;

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
            new { Id = SpeciesIds.Dog, Name = "Dog", CreatedBy = Guid.Empty },
            new { Id = SpeciesIds.Cat, Name = "Cat", CreatedBy = Guid.Empty },
            new { Id = SpeciesIds.Horse, Name = "Horse", CreatedBy = Guid.Empty });
    }
}
