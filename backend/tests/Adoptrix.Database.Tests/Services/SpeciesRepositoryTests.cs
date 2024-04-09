using Adoptrix.Application.Services.Repositories;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class SpeciesRepositoryTests(DatabaseFixture fixture)
{
    private readonly ISpeciesRepository repository = fixture.SpeciesRepository!;

    [Fact]
    public async Task GetAllAsync_WithNoParameters_ShouldReturnAllSpecies()
    {
        // act
        var results = await repository.GetAllAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(GetKnownSpecies))]
    public async Task GetByIdAsync_WithValidId_ShouldReturnExpectedResult(Guid speciesId, string expectedName)
    {
        // act
        var species = await repository.GetByIdAsync(speciesId);

        // assert
        species.Should().BeOfType<Species>().Which.Name.Should().Be(expectedName);
    }

    [Theory]
    [MemberData(nameof(GetKnownSpecies))]
    public async Task GetByNameAsync_WithValidName_ShouldReturnExpectedResult(Guid expectedId, string speciesName)
    {
        // act
        var species = await repository.GetByNameAsync(speciesName);

        // assert
        species.Should().BeOfType<Species>().Which.Id.Should().Be(expectedId);
    }

    public static TheoryData<Guid, string> GetKnownSpecies() => new()
    {
        { SpeciesIds.Dog, "Dog" },
        { SpeciesIds.Cat, "Cat" },
        { SpeciesIds.Horse, "Horse" }
    };
}
