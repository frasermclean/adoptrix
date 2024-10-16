namespace Adoptrix.Core.Factories;

public static class AnimalFactory
{
    public static Animal Create(string name, string? description = null, Breed? breed = null,
        Sex sex = Sex.Male, DateOnly dateOfBirth = default) => new()
    {
        Name = name,
        Description = description,
        Breed = breed ?? BreedFactory.Create(),
        Sex = sex,
        DateOfBirth = dateOfBirth,
        Slug = Animal.CreateSlug(name, dateOfBirth)
    };
}
