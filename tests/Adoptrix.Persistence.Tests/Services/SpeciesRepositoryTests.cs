using Adoptrix.Core;
using Adoptrix.Initializer;
using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class SpeciesRepositoryTests
{
    private readonly ISpeciesRepository speciesRepository;

    public SpeciesRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();
        (_, _, speciesRepository) = collection;
    }

    [Fact]
    public async Task SearchAsync_WithEmptyQuery_ShouldReturnAllSpecies()
    {
        // act
        var results = await speciesRepository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterThan(0);
    }

    [Theory]
    [MemberData(nameof(SeededSpecies))]
    public async Task GetByIdAsync_WithValidId_ShouldReturnExpectedResult(string speciesId, string expectedName)
    {
        // act
        var species = await speciesRepository.GetByIdAsync(Guid.Parse(speciesId));

        // assert
        species.Should().BeOfType<Species>().Which.Name.Should().Be(expectedName);
    }

    [Theory]
    [MemberData(nameof(SeededSpecies))]
    public async Task GetByNameAsync_WithValidName_ShouldReturnExpectedResult(string expectedId, string speciesName)
    {
        // act
        var species = await speciesRepository.GetByNameAsync(speciesName);

        // assert
        species.Should().BeOfType<Species>().Which.Id.Should().Be(expectedId);
    }

    public static readonly TheoryData<string, string> SeededSpecies = new()
    {
        { SeedData.Species["Dog"].Id.ToString(), "Dog" },
        { SeedData.Species["Cat"].Id.ToString(), "Cat" },
        { SeedData.Species["Bird"].Id.ToString(), "Bird" }
    };
}
