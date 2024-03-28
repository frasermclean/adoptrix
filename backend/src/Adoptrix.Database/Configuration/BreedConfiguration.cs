using Adoptrix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Database.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasIndex(breed => breed.Name)
            .IsUnique();

        builder.Property(breed => breed.Name)
            .HasColumnType("nvarchar")
            .HasMaxLength(Breed.NameMaxLength);

        builder.Property(breed => breed.CreatedAt)
            .HasColumnType("datetime2")
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.HasData(
            new
            {
                Id = Guid.Parse("e719fea1-14d5-45a3-845d-404c88c4549f"),
                Name = "Labrador Retriever",
                SpeciesId = Species.DogSpeciesId,
                CreatedBy = Guid.Empty
            },
            new
            {
                Id = Guid.Parse("9b2ace0b-fb18-4da4-86a5-c7404cfbf145"),
                Name = "German Shepherd",
                SpeciesId = Species.DogSpeciesId,
                CreatedBy = Guid.Empty
            },
            new
            {
                Id = Guid.Parse("4fb1e168-bf13-4702-9b61-0b8df2ef0c7d"),
                Name = "Golden Retriever",
                SpeciesId = Species.DogSpeciesId,
                CreatedBy = Guid.Empty
            }
        );
    }
}