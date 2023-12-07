using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.EntityGenerators;

public static class AnimalGenerator
{
    private static readonly Faker<Animal> AnimalFaker = new Faker<Animal>()
        .RuleFor(animal => animal.Id, faker => faker.Random.Int(1, 1000))
        .RuleFor(animal => animal.Name, faker => faker.Name.FirstName())
        .RuleFor(animal => animal.Description, faker => faker.Lorem.Paragraph())
        .RuleFor(animal => animal.Species, SpeciesGenerator.Generate)
        .RuleFor(animal => animal.Breed, BreedGenerator.Generate)
        .RuleFor(animal => animal.Sex, faker => faker.Random.Enum<Sex>())
        .RuleFor(animal => animal.DateOfBirth, faker => faker.Date.PastDateOnly(10));

    public static Animal Generate() => AnimalFaker.Generate();
    public static List<Animal> Generate(int count) => AnimalFaker.Generate(count);
}