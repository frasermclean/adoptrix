using Adoptrix.Api.Mapping;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Api.Tests.Mapping;

public class SpeciesResponseMapperTests
{
    [Fact]
    public void MappingSpeciesToSpeciesResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var species = SpeciesFactory.Create();

        // act
        var response = species.ToResponse();

        // assert
        response.Id.Should().Be(species.Id);
        response.Name.Should().Be(species.Name);
    }
}
