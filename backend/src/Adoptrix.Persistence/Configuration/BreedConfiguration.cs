using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.Property(breed => breed.Name)
            .HasMaxLength(Breed.NameMaxLength);

        builder.Property(breed => breed.LastModifiedUtc)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()")
            .HasConversion<UtcDateTimeConverter>();

        builder.HasIndex(breed => breed.Name)
            .IsUnique();

        builder.HasData(
            new
            {
                Id = SeedData.Breeds.FrenchBulldog,
                Name = "French Bulldog",
                SpeciesId = SeedData.Species.Dog,
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Breeds.LabradorRetriever,
                Name = "Labrador retriever",
                SpeciesId = SeedData.Species.Dog,
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Breeds.GermanShepherd,
                Name = "German shepherd",
                SpeciesId = SeedData.Species.Dog,
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Breeds.GoldenRetriever,
                Name = "Golden Retriever",
                SpeciesId = SeedData.Species.Dog,
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Breeds.DomesticShorthair,
                Name = "Domestic Shorthair",
                SpeciesId = SeedData.Species.Cat,
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Breeds.AfricanGreyParrot,
                Name = "African Grey Parrot",
                SpeciesId = SeedData.Species.Bird,
                LastModifiedUtc = DateTime.MinValue
            }
        );
    }
}
