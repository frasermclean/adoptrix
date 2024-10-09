using Adoptrix.Core;

namespace Adoptrix.Initializer;

public static class SeedData
{
    public static readonly Species[] Species =
    [
        new Species
        {
            Name = "Dog"
        },
        new Species
        {
            Name = "Cat"
        },
        new Species
        {
            Name = "Bird"
        }
    ];

    public static readonly Breed[] Breeds =
    [
        new Breed
        {
            Name = "Labrador Retriever",
            Species = Species[0]
        },
        new Breed
        {
            Name = "German Shepherd",
            Species = Species[0]
        },
        new Breed
        {
            Name = "Golden Retriever",
            Species = Species[0]
        },
        new Breed
        {
            Name = "Domestic Short-hair",
            Species = Species[1]
        },
        new Breed
        {
            Name = "African Grey Parrot",
            Species = Species[2]
        }
    ];

    public static readonly Animal[] Animals =
    [
        new Animal
        {
            Name = "Alberto",
            Description =
                "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! \ud83d\udc3e #AdoptAlberto",
            Breed = Breeds[0],
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2024, 02, 14),
            Slug = Animal.CreateSlug("Alberto", new DateOnly(2024, 02, 14))
        },
        new Animal
        {
            Name = "Barry",
            Description =
                "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! \ud83d\udc3e #AdoptBarry",
            Breed = Breeds[1],
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2020, 04, 19),
            Slug = Animal.CreateSlug("Barry", new DateOnly(2020, 04, 19))
        },
        new Animal
        {
            Name = "Ginger",
            Description =
                "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger",
            Breed = Breeds[3],
            Sex = Sex.Female,
            DateOfBirth = new DateOnly(2022, 09, 30),
            Slug = Animal.CreateSlug("Ginger", new DateOnly(2022, 09, 30))

        },
        new Animal
        {
            Name = "Percy",
            Description = "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.",
            Breed = Breeds[4],
            Sex = Sex.Male,
            DateOfBirth = new DateOnly(2017, 04, 11),
            Slug = Animal.CreateSlug("Percy", new DateOnly(2017, 04, 11))
        }
    ];
}
