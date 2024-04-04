namespace Adoptrix.Domain.Models.Factories;

public static class AnimalFactory
{
    private static readonly string[] Names = ["Buddy", "Max", "Bella", "Lucy", "Charlie", "Daisy", "Bailey", "Molly"];

    public static Animal Create(Guid? id = null, string? name = null, Breed? breed = null, Sex sex = Sex.Male,
        DateOnly? dateOfBirth = null, int imageCount = 0, Guid? createdBy = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name ?? Names[Random.Shared.Next(Names.Length)],
        Breed = breed ?? BreedFactory.Create(),
        Sex = sex,
        DateOfBirth = dateOfBirth ?? DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(365 * 2)),
        Images = AnimalImageFactory.CreateMany(imageCount).ToList(),
        CreatedBy = createdBy ?? Guid.NewGuid()
    };

    public static IEnumerable<Animal> CreateMany(int count)
        => Enumerable.Range(0, count).Select(_ => Create());
}
