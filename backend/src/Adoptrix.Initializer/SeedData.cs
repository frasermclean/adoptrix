using Adoptrix.Core;

namespace Adoptrix.Initializer;

public static class SeedData
{
    private static readonly Species Dog = new("Dog");
    private static readonly Species Cat = new("Cat");
    private static readonly Species Bird = new("Bird");

    public static readonly Species[] AllSpecies = [Dog, Cat, Bird];

    private static readonly Breed LabradorRetriever = Breed.Create("Labrador Retriever", Dog);
    private static readonly Breed GermanShepherd = Breed.Create("German Shepherd", Dog);
    private static readonly Breed GoldenRetriever = Breed.Create("Golden Retriever", Dog);
    private static readonly Breed DomesticShortHair = Breed.Create("Domestic Short-hair", Cat);
    private static readonly Breed AfricanGreyParrot = Breed.Create("African Grey Parrot", Bird);

    public static readonly Breed[] AllBreeds =
        [LabradorRetriever, GermanShepherd, GoldenRetriever, DomesticShortHair, AfricanGreyParrot];

    public static readonly Animal Alberto = Animal.Create(
        name: "Alberto",
        description:
        "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! \ud83d\udc3e #AdoptAlberto",
        breed: LabradorRetriever,
        sex: Sex.Male,
        dateOfBirth: new DateOnly(2024, 02, 14));

    public static readonly Animal Barry = Animal.Create(
        name: "Barry",
        description:
        "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! \ud83d\udc3e #AdoptBarry",
        breed: GermanShepherd,
        sex: Sex.Male,
        dateOfBirth: new DateOnly(2020, 04, 19));

    public static readonly Animal Ginger = Animal.Create(
        name: "Ginger",
        description:
        "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger",
        breed: DomesticShortHair,
        sex: Sex.Female,
        dateOfBirth: new DateOnly(2022, 09, 30));

    public static readonly Animal Percy = Animal.Create(
        name: "Percy",
        description: "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.",
        breed: AfricanGreyParrot,
        sex: Sex.Male,
        dateOfBirth: new DateOnly(2017, 04, 11));


    public static readonly Animal[] AllAnimals =
    [
        Alberto,
        Barry,
        Ginger,
        Percy
    ];
}
