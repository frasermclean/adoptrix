using Adoptrix.Application.Services.Repositories;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class SpeciesRepositoryTests
{
    private readonly ISpeciesRepository speciesRepository;
    public SpeciesRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();

        speciesRepository = collection.SpeciesRepository;
    }

    [Fact]
    public async Task GetAllAsync_WithNoParameters_ShouldReturnAllSpecies()
    {
        // act
        var results = await speciesRepository.GetAllAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(GetKnownSpecies))]
    public async Task GetByIdAsync_WithValidId_ShouldReturnExpectedResult(Guid speciesId, string expectedName)
    {
        // act
        var species = await speciesRepository.GetByIdAsync(speciesId);

        // assert
        species.Should().BeOfType<Species>().Which.Name.Should().Be(expectedName);
    }

    [Theory]
    [MemberData(nameof(GetKnownSpecies))]
    public async Task GetByNameAsync_WithValidName_ShouldReturnExpectedResult(Guid expectedId, string speciesName)
    {
        // act
        var species = await speciesRepository.GetByNameAsync(speciesName);

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
