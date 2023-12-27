using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Infrastructure.Data.Configuration;

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
            new { Id = Species.DogSpeciesId, Name = "Dog", CreatedBy = Guid.Empty },
            new { Id = Species.CatSpeciesId, Name = "Cat", CreatedBy = Guid.Empty },
            new { Id = Species.HorseSpeciesId, Name = "Horse", CreatedBy = Guid.Empty });
    }
}