using Adoptrix.Core.Extensions;
using Adoptrix.Core.Requests;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Core.Tests.Extensions;

public class AddBreedRequestExtensionsTests
{
    [Fact]
    public void ToBreed_WithValidRequest_ReturnsBreed()
    {
        // arrange
        var request = new AddBreedRequest
        {
            Name = "Iguana",
            SpeciesName = "Reptile",
            UserId = Guid.NewGuid()
        };
        var species = SpeciesFactory.Create();

        // act
        var result = request.ToBreed(species);

        // assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Species.Should().Be(species);
        result.LastModifiedBy.Should().Be(request.UserId);
    }
}
