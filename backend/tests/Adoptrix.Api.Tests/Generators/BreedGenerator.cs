using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class BreedGenerator
{
    private static readonly Faker<Breed> BreedFaker = new Faker<Breed>()
        .RuleFor(breed => breed.Name, faker => faker.Name.FirstName())
        .RuleFor(breed => breed.Species, SpeciesGenerator.Generate);
        //.RuleFor(breed => breed.Animals, AnimalGenerator.Generate(3));

    public static Breed Generate() => BreedFaker.Generate();
}