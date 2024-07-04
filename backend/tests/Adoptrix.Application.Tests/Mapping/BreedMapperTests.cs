using Adoptrix.Tests.Shared.Factories;
using BreedMapper = Adoptrix.Application.Mapping.BreedMapper;

namespace Adoptrix.Application.Tests.Mapping;

public class BreedMapperTests
{
    [Fact]
    public void MappingBreedToBreedResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var breed = BreedFactory.Create();

        // act
        var response = BreedMapper.ToResponse(breed);

        // assert
        response.Id.Should().Be(breed.Id);
        response.Name.Should().Be(breed.Name);
        response.SpeciesId.Should().Be(breed.Species.Id);
    }
}
