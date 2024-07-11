using Adoptrix.Mapping;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mapping;

public class AnimalMapperTests
{
    [Fact]
    public void MappingAnimalToAnimalResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var animal = AnimalFactory.Create(imageCount: 3);

        // act
        var response = animal.ToResponse();

        // assert
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.SpeciesId.Should().Be(animal.Breed.Species.Id);
        response.SpeciesName.Should().Be(animal.Breed.Species.Name);
        response.BreedId.Should().Be(animal.Breed.Id);
        response.BreedName.Should().Be(animal.Breed.Name);
        response.Sex.Should().Be(animal.Sex);
        response.DateOfBirth.Should().Be(animal.DateOfBirth);
        response.CreatedAt.Should().Be(animal.CreatedAt.ToUniversalTime());
        response.Images.Should().HaveCount(animal.Images.Count).And.AllSatisfy(imageResponse =>
        {
            imageResponse.Id.Should().NotBeEmpty();
            imageResponse.Description.Should().NotBeEmpty();
        });
    }
}
