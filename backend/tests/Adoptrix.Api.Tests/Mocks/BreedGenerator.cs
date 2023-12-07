using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Mocks;

public static class BreedGenerator
{
    private static Faker<Breed> BreedFaker = new Faker<Breed>()
        .RuleFor(breed => breed.Name, faker => faker.Name.FirstName())
        .RuleFor(breed => breed.Species, SpeciesGenerator.Generate);

    public static Breed Generate() => BreedFaker.Generate();
}