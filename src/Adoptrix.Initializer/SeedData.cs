using Adoptrix.Core;

namespace Adoptrix.Initializer;

public static class SeedData
{
    public static readonly Dictionary<string, Species> Species = new()
    {
        { "Dog", new Species { Name = "Dog" } },
        { "Cat", new Species { Name = "Cat" } },
        { "Bird", new Species { Name = "Bird" } }
    };

    public static readonly Dictionary<string, Breed> Breeds = new()
    {
        {
            "Labrador Retriever", new Breed
            {
                Id = 1,
                Name = "Labrador Retriever",
                Species = Species["Dog"]
            }
        },
        {
            "German Shepherd", new Breed
            {
                Id = 2,
                Name = "German Shepherd",
                Species = Species["Dog"]
            }
        },
        {
            "Golden Retriever", new Breed
            {
                Id = 3,
                Name = "Golden Retriever",
                Species = Species["Dog"]
            }
        },
        {
            "Domestic Shorthair", new Breed
            {
                Id = 4,
                Name = "Domestic Shorthair",
                Species = Species["Cat"]
            }
        },
        {
            "African Grey Parrot", new Breed
            {
                Id = 5,
                Name = "African Grey Parrot",
                Species = Species["Bird"]
            }
        }
    };

    public static readonly Animal[] Animals =
    [
        new Animal
        {
            Id = 1,
            Name = "Alberto",
            Description = """
                          Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat
                          that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies
                          to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a
                          family to call his own, where he can share his boundless love and enthusiasm. Could you be
                          the one to open your heart and home to Alberto, making his dreams come true? Adopt this
                          charming ball of fur, and let the adventure of a lifetime begin! 🐾 #AdoptAlberto"
                          """,
            Breed = Breeds["Labrador Retriever"],
            Sex = Sex.Male,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(165)),
        },
        new Animal
        {
            Id = 2,
            Name = "Barry",
            Description = """
                          Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old,
                          Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in
                          the sunlight as he explores the world with curiosity and confidence. From romping through
                          fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and
                          companionship. This noble canine seeks a forever home where he can shower his family with
                          unconditional love and protection. Ready to welcome a steadfast friend into your life?
                          Consider adopting Barry, and embark on a journey of trust, devotion, and endless
                          adventure! 🐾 #AdoptBarry
                          """,
            Breed = Breeds["German Shepherd"],
            Sex = Sex.Male,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * 4.1)),
        },
        new Animal
        {
            Id = 3,
            Name = "Ginger",
            Description = """
                          Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful
                          spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes.
                          From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and
                          mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot
                          and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your
                          life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger
                          """,
            Breed = Breeds["Domestic Shorthair"],
            Sex = Sex.Female,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * 2.3)),
        },
        new Animal
        {
            Id = 4,
            Name = "Percy",
            Description = """
                          Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.
                          """,
            Breed = Breeds["African Grey Parrot"],
            Sex = Sex.Male,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * 7.5)),
        }
    ];
}
