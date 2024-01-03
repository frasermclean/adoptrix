using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Generators;

public static class SpeciesGenerator
{
    private static readonly Faker<Species> SpeciesFaker = new Faker<Species>()
        .RuleFor(species => species.Id, faker => faker.Random.Guid())
        .RuleFor(species => species.Name, faker => faker.Random.String(3, Species.NameMaxLength, 'A', 'z'))
        .RuleFor(species => species.CreatedAt, faker => faker.Date.Past())
        .RuleFor(species => species.CreatedBy, faker => faker.Random.Guid());

    public static Species Generate() => SpeciesFaker.Generate();
}