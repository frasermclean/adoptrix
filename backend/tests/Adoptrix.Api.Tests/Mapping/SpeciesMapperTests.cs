using Adoptrix.Api.Mapping;
using Adoptrix.Api.Tests.Generators;

namespace Adoptrix.Api.Tests.Mapping;

public class SpeciesMapperTests
{
    [Fact]
    public void MappingSpeciesToSpeciesResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var species = SpeciesGenerator.Generate();

        // act
        var response = species.ToResponse();

        // assert
        response.Id.Should().Be(species.Id);
        response.Name.Should().Be(species.Name);
    }
}