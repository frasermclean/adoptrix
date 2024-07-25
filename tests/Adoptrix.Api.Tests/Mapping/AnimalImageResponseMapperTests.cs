using Adoptrix.Api.Mapping;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Api.Tests.Mapping;

public class AnimalImageResponseMapperTests
{
    [Fact]
    public void Mapping_ImageInformation_To_ImageResponse_Should_Return_ExpectedValues()
    {
        // arrange
        var animalImage = AnimalImageFactory.Create();

        // act
        var response = animalImage.ToResponse();

        // assert
        response.Id.Should().Be(animalImage.Id);
        response.Description.Should().Be(animalImage.Description);
        response.IsProcessed.Should().Be(animalImage.IsProcessed);
    }
}
