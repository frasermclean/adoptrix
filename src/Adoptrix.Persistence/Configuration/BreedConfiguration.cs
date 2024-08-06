using Adoptrix.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.Property(breed => breed.Name)
            .HasMaxLength(Breed.NameMaxLength);

        builder.Property(breed => breed.CreatedAt)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(animal => animal.CreatedBy)
            .HasDefaultValue(Guid.Empty);

        builder.HasIndex(breed => breed.Name)
            .IsUnique();

        builder.HasData(InitialData);
    }

    private static readonly object[] InitialData =
    [
        new
        {
            Id = 1,
            Name = "Labrador Retriever",
            SpeciesName = "Dog"
        },
        new
        {
            Id = 2,
            Name = "German Shepherd",
            SpeciesName = "Dog"
        },
        new
        {
            Id = 3,
            Name = "Golden Retriever",
            SpeciesName = "Dog"
        },
        new
        {
            Id = 4,
            Name = "Domestic Shorthair",
            SpeciesName = "Cat"
        },
        new
        {
            Id = 5,
            Name = "African Grey Parrot",
            SpeciesName = "Bird"
        }
    ];
}
