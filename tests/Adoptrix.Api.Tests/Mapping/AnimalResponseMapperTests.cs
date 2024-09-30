using Adoptrix.Logic.Mapping;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Api.Tests.Mapping;

public class AnimalResponseMapperTests
{
    [Fact]
    public void ToResponse_WithValidInput_ShouldReturnExpectedValues()
    {
        // arrange
        var animal = AnimalFactory.Create(imageCount: 3);

        // act
        var response = animal.ToResponse();

        // assert
        response.Id.Should().Be(animal.Id);
        response.Name.Should().Be(animal.Name);
        response.Description.Should().Be(animal.Description);
        response.SpeciesName.Should().Be(animal.Breed.Species.Name);
        response.BreedName.Should().Be(animal.Breed.Name);
        response.Sex.Should().Be(animal.Sex);
        response.DateOfBirth.Should().Be(animal.DateOfBirth);
        response.LastModifiedUtc.Should().Be(animal.LastModifiedUtc.ToUniversalTime());
        response.Images.Should().HaveCount(animal.Images.Count).And.AllSatisfy(imageResponse =>
        {
            imageResponse.Description.Should().NotBeEmpty();
        });
    }
}
