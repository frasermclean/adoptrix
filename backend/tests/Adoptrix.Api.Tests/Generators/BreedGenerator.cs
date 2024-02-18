using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class BreedGenerator
{
    public static Breed Generate(string? breedName = null) => CreateFaker(breedName).Generate();

    private static Faker<Breed> CreateFaker(string? breedName) => new Faker<Breed>()
        .RuleFor(breed => breed.Name, faker => breedName ?? faker.Name.FirstName())
        .RuleFor(breed => breed.Species, SpeciesGenerator.Generate);
}
