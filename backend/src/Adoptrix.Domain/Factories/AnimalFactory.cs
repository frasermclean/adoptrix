namespace Adoptrix.Domain.Factories;

public static class AnimalFactory
{
    public static Animal CreateAnimal(Guid? id = null, string name = "Buddy", Breed? breed = null, Sex sex = Sex.Male,
        DateOnly? dateOfBirth = null, int imageCount = 0, Guid? createdBy = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name,
        Breed = breed ?? BreedFactory.CreateBreed(),
        Sex = sex,
        DateOfBirth = dateOfBirth ?? DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(365 * 2)),
        Images = Enumerable.Range(0, imageCount)
            .Select(num => AnimalImageFactory.CreateAnimalImage(originalFileName: $"image{num}.jpg"))
            .ToList(),
        CreatedBy = createdBy ?? Guid.NewGuid()
    };
}
