namespace Adoptrix.Core.Factories;

public static class AnimalFactory
{
    private static readonly string[] Names = ["Buddy", "Max", "Bella", "Lucy", "Charlie", "Daisy", "Bailey", "Molly"];

    public static Animal Create(string? name = null, string? description = null, Breed? breed = null,
        Sex sex = Sex.Male, DateOnly? dateOfBirth = null)
    {
        name ??= Names[Random.Shared.Next(Names.Length)];
        dateOfBirth ??= DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(365 * 2));

        return new Animal
        {
            Name = name,
            Description = description,
            Breed = breed ?? BreedFactory.Create(),
            Sex = sex,
            DateOfBirth = dateOfBirth.Value,
            Slug = Animal.CreateSlug(name, dateOfBirth.Value)
        };
    }
}
