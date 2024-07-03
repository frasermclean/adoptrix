using Adoptrix.Tests.Shared.Factories;
using SpeciesMapper = Adoptrix.Application.Mapping.SpeciesMapper;

namespace Adoptrix.Api.Tests.Mapping;

public class SpeciesMapperTests
{
    [Fact]
    public void MappingSpeciesToSpeciesResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var species = SpeciesFactory.Create();

        // act
        var response = SpeciesMapper.ToResponse(species);

        // assert
        response.Id.Should().Be(species.Id);
        response.Name.Should().Be(species.Name);
    }
}
