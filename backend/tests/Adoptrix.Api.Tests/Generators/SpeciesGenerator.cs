using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class SpeciesGenerator
{
    private static readonly Faker<Species> SpeciesFaker = new Faker<Species>()
        .RuleFor(species => species.Id, Guid.NewGuid)
        .RuleFor(species => species.Name, faker => faker.Lorem.Word());

    public static Species Generate() => SpeciesFaker.Generate();
}