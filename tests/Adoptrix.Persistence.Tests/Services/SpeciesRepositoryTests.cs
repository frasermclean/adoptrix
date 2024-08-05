using Adoptrix.Core;
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
    [InlineData("Dog")]
    public async Task GetAsync_WithValidName_ShouldReturnExpectedResult(string speciesName)
    {
        // act
        var species = await speciesRepository.GetAsync(speciesName);

        // assert
        species.Should().BeOfType<Species>().Which.Name.Should().Be(speciesName);
    }
}
