using Adoptrix.Mapping;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mapping;

public class SpeciesMapperTests
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
