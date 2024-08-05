using Adoptrix.Core;
using Adoptrix.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adoptrix.Persistence.Configuration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(animal => animal.Name)
            .HasMaxLength(Animal.NameMaxLength);

        builder.Property(animal => animal.Sex)
            .HasConversion<SexConverter>();

        builder.Property(animal => animal.CreatedAt)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.HasQueryFilter(animal => !animal.IsDeleted);

        builder.HasData(InitialData);

        var animalImagesBuilder = builder.OwnsMany(animal => animal.Images)
            .ToTable("AnimalImages");

        ConfigureAnimalImagesTable(animalImagesBuilder);
    }

    private static void ConfigureAnimalImagesTable(OwnedNavigationBuilder<Animal, AnimalImage> builder)
    {
        builder.Property(imageInformation => imageInformation.UploadedAt)
            .HasPrecision(2)
            .HasDefaultValueSql("getutcdate()");

        builder.Property(imageInformation => imageInformation.OriginalFileName)
            .HasMaxLength(512);

        builder.Property(imageInformation => imageInformation.OriginalContentType)
            .HasMaxLength(AnimalImage.ContentTypeMaxLength);
    }

    private static readonly object[] InitialData =
    [
        new
        {
            Id = 1,
            Name = "Alberto",
            Description =
                "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! \ud83d\udc3e #AdoptAlberto",
            BreedId = 1,
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2024, 02, 14),
            IsDeleted = false
        },
        new
        {
            Id = 2,
            Name = "Barry",
            Description =
                "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! \ud83d\udc3e #AdoptBarry",
            BreedId = 2,
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2020, 04, 19),
            IsDeleted = false
        },
        new
        {
            Id = 3,
            Name = "Ginger",
            Description =
                "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger",
            BreedId = 4,
            Sex = Sex.Female,
            DateOfBirth = new DateOnly(2022, 09, 30),
            IsDeleted = false
        },
        new
        {
            Id = 4,
            Name = "Percy",
            Description = "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.",
            BreedId = 5,
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2017, 04, 11),
            IsDeleted = false
        }
    ];
}
