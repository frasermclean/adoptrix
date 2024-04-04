using Adoptrix.Api.Mapping;
using Adoptrix.Domain.Models.Factories;

namespace Adoptrix.Api.Tests.Mapping;

public class BreedMapperTests
{
    [Fact(Skip = "Circular references in generators")]
    public void MappingBreedToBreedResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var breed = BreedFactory.Create();

        // act
        var response = breed.ToResponse();

        // assert
        response.Id.Should().Be(breed.Id);
        response.Name.Should().Be(breed.Name);
        response.SpeciesId.Should().Be(breed.Species.Id);
        response.AnimalIds.Should().HaveCount(breed.Animals.Count).And.Contain(breed.Animals.Select(animal => animal.Id));
    }
}
