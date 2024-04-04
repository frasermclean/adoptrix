using Adoptrix.Domain.Models;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class BreedGenerator
{
    public static Breed Generate(Guid? breedId = null, string? breedName = null) =>
        CreateFaker(breedId, breedName).Generate();

    private static Faker<Breed> CreateFaker(Guid? breedId = null, string? breedName = null) => new Faker<Breed>()
        .RuleFor(breed => breed.Id, faker => breedId ?? faker.Random.Guid())
        .RuleFor(breed => breed.Name, faker => breedName ?? faker.Name.FirstName())
        .RuleFor(breed => breed.Species, SpeciesGenerator.Generate);
}
