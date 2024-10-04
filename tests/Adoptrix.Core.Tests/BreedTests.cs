using Adoptrix.Core.Requests;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Core.Tests;

public class BreedTests
{
    [Fact]
    public void Update_WithValidParameters_ShouldUpdateProperties()
    {
        // arrange
        var breed = BreedFactory.Create();
        var request = new UpdateBreedRequest
        {
            Name = "Sphinx",
            SpeciesName = "Cat",
            UserId = Guid.NewGuid()
        };
        var species = SpeciesFactory.Create();

        // act
        breed.Update(request, species);

        // assert
        breed.Name.Should().Be(request.Name);
        breed.Species.Should().Be(species);
        breed.LastModifiedBy.Should().Be(request.UserId);
        breed.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
