using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Mocks;

public static class SpeciesGenerator
{
    private static readonly Faker<Species> SpeciesFaker = new Faker<Species>()
        .RuleFor(species => species.Id, faker => faker.Random.Int(1, 1000))
        .RuleFor(species => species.Name, faker => faker.Lorem.Word());

    public static Species Generate() => SpeciesFaker.Generate();
}