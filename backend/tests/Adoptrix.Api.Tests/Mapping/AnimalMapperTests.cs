using Adoptrix.Api.Mapping;
using Adoptrix.Api.Tests.Generators;

namespace Adoptrix.Api.Tests.Mapping;

public class AnimalMapperTests
{
    [Fact]
    public void MappingAnimalToAnimalResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var animal = AnimalGenerator.Generate();

        // act
        var response = animal.ToResponse();

        // assert
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.Species.Id.Should().Be(animal.Species.Id);
        response.Species.Name.Should().Be(animal.Species.Name);
        response.Breed.Id.Should().Be(animal.Breed.Id);
        response.Breed.Name.Should().Be(animal.Breed.Name);
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
