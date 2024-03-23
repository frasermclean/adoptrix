using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Storage.Tests.Fixtures;

namespace Adoptrix.Infrastructure.Storage.Tests.Services.Repositories;

[Trait("Category", "Integration")]
public class SpeciesRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly ISpeciesRepository repository = fixture.SpeciesRepository!;

    [Theory]
    [InlineData("c9c1836b-1051-45c3-a2c4-0d841e69e6d3", "Cat")]
    [InlineData("f2d44dc6-6c6c-41d0-ad8d-e6c814b09c1a", "Dog")]
    [InlineData("e6d11a53-bacb-4a8b-a171-beea7e935467", "Horse")]
    public async Task GetByIdAsync_WithValidId_Should_Return_ExpectedResult(Guid speciesId, string expectedName)
    {
        // act
        var result = await repository.GetByIdAsync(speciesId);

        // assert
        result.Should().BeSuccess();
        result.Value.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("Cat", "c9c1836b-1051-45c3-a2c4-0d841e69e6d3")]
    [InlineData("Dog", "f2d44dc6-6c6c-41d0-ad8d-e6c814b09c1a")]
    [InlineData("Horse", "e6d11a53-bacb-4a8b-a171-beea7e935467")]
    public async Task GetByNameAsync_WithValidName_Should_Return_ExpectedResult(string name, Guid expectedId)
    {
        // act
        var result = await repository.GetByNameAsync(name);

        // assert
        result.Should().BeSuccess();
        result.Value.Id.Should().Be(expectedId);
    }
}
