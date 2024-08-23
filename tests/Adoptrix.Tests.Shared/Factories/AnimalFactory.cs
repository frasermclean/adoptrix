using Adoptrix.Core;

namespace Adoptrix.Tests.Shared.Factories;

public static class AnimalFactory
{
    private static readonly string[] Names = ["Buddy", "Max", "Bella", "Lucy", "Charlie", "Daisy", "Bailey", "Molly"];

    public static Animal Create(int? id = null, string? name = null, Breed? breed = null, Sex sex = Sex.Male,
        DateOnly? dateOfBirth = null, int imageCount = 0, Guid? createdBy = null)
    {
        name ??= Names[Random.Shared.Next(Names.Length)];
        dateOfBirth ??= DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(365 * 2));

        return new Animal
        {
            Id = id ?? default,
            Name = name,
            Breed = breed ?? BreedFactory.Create(),
            Sex = sex,
            DateOfBirth = dateOfBirth.Value,
            Slug = Animal.CreateSlug(name, dateOfBirth.Value),
            Images = AnimalImageFactory.CreateMany(imageCount).ToList(),
            CreatedBy = createdBy ?? Guid.NewGuid()
        };
    }

    public static IEnumerable<Animal> CreateMany(int count)
        => Enumerable.Range(0, count).Select(_ => Create());
}
