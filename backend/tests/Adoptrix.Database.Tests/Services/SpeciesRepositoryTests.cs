using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class SpeciesRepositoryTests(DatabaseFixture fixture)
{
    private readonly ISpeciesRepository repository = fixture.SpeciesRepository!;

    [Theory]
    [ClassData(typeof(SpeciesData))]
    public async Task GetByIdAsync_WithValidId_Should_Return_ExpectedResult(Guid speciesId, string expectedName)
    {
        // act
        var result = await repository.GetByIdAsync(speciesId);

        // assert
        result.Should().BeSuccess();
        result.Value.Name.Should().Be(expectedName);
    }

    [Theory]
    [ClassData(typeof(SpeciesData))]
    public async Task GetByNameAsync_WithValidName_Should_Return_ExpectedResult(Guid expectedId, string speciesName)
    {
        // act
        var result = await repository.GetByNameAsync(speciesName);

        // assert
        result.Should().BeSuccess();
        result.Value.Id.Should().Be(expectedId);
    }
}
