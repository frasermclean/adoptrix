using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Infrastructure.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasIndex(breed => breed.Name)
            .IsUnique();

        builder.Property(breed => breed.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Breed.NameMaxLength);

        builder.HasData(
            new { Id = 1, Name = "Labrador Retriever", SpeciesId = 1 },
            new { Id = 2, Name = "German Shepherd", SpeciesId = 1 },
            new { Id = 3, Name = "Golden Retriever", SpeciesId = 1 }
        );
    }
}