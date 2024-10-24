using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(animal => animal.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(animal => animal.Name)
            .HasMaxLength(Animal.NameMaxLength);

        builder.Property(animal => animal.Description)
            .HasMaxLength(Animal.DescriptionMaxLength);

        builder.Property(animal => animal.Sex)
            .HasConversion<SexConverter>();

        builder.Property(animal => animal.LastModifiedUtc)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()")
            .HasConversion<UtcDateTimeConverter>();

        builder.Property(animal => animal.Slug)
            .HasMaxLength(Animal.SlugMaxLength);

        builder.HasAlternateKey(animal => animal.Slug);

        builder.OwnsMany(animal => animal.Images, imagesBuilder =>
        {
            imagesBuilder.ToTable("AnimalImages");

            imagesBuilder.WithOwner()
                .HasForeignKey(animalImage => animalImage.AnimalSlug)
                .HasPrincipalKey(animal => animal.Slug);

            imagesBuilder.Property(animalImage => animalImage.Id)
                .HasDefaultValueSql("newid()");

            imagesBuilder.Property(animalImage => animalImage.LastModifiedUtc)
                .HasPrecision(2)
                .HasDefaultValueSql("getutcdate()")
                .HasConversion<UtcDateTimeConverter>();

            imagesBuilder.Property(animalImage => animalImage.OriginalFileName)
                .HasMaxLength(512);

            imagesBuilder.Property(animalImage => animalImage.OriginalContentType)
                .HasMaxLength(AnimalImage.ContentTypeMaxLength);
        });

        builder.HasData(
            new
            {
                Id = SeedData.Animals.Alberto,
                Name = "Alberto",
                Description =
                    "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! \ud83d\udc3e #AdoptAlberto",
                BreedId = SeedData.Breeds.LabradorRetriever,
                Sex = Sex.Male,
                DateOfBirth = new DateOnly(2024, 02, 14),
                Slug = Animal.CreateSlug("Alberto", new DateOnly(2024, 02, 14)),
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Animals.Barry,
                Name = "Barry",
                Description =
                    "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! \ud83d\udc3e #AdoptBarry",
                BreedId = SeedData.Breeds.GermanShepherd,
                Sex = Sex.Male,
                DateOfBirth = new DateOnly(2020, 04, 19),
                Slug = Animal.CreateSlug("Barry", new DateOnly(2020, 04, 19)),
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Animals.Ginger,
                Name = "Ginger",
                Description =
                    "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger",
                BreedId = SeedData.Breeds.DomesticShorthair,
                Sex = Sex.Female,
                DateOfBirth = new DateOnly(2022, 09, 30),
                Slug = Animal.CreateSlug("Ginger", new DateOnly(2022, 09, 30)),
                LastModifiedUtc = DateTime.MinValue
            },
            new
            {
                Id = SeedData.Animals.Percy,
                Name = "Percy",
                Description =
                    "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.",
                BreedId = SeedData.Breeds.AfricanGreyParrot,
                Sex = Sex.Female,
                DateOfBirth = new DateOnly(2017, 04, 11),
                Slug = Animal.CreateSlug("Percy", new DateOnly(2017, 04, 11)),
                LastModifiedUtc = DateTime.MinValue
            }
        );
    }
}
