using Adoptrix.Tests.Shared.Factories;
using BreedResponseMapper = Adoptrix.Logic.Mapping.BreedResponseMapper;

namespace Adoptrix.Api.Tests.Mapping;

public class BreedResponseMapperTests
{
    [Fact]
    public void MappingBreedToBreedResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var breed = BreedFactory.Create();

        // act
        var response = BreedResponseMapper.ToResponse(breed);

        // assert
        response.Id.Should().Be(breed.Id);
        response.Name.Should().Be(breed.Name);
        response.SpeciesName.Should().Be(breed.Species.Name);
    }
}
