using Adoptrix.Api.Mapping;
using Adoptrix.Api.Tests.Generators;

namespace Adoptrix.Api.Tests.Mapping;

public class ImageInformationMapperTests
{
    [Fact]
    public void MappingImageInformationToAnimalImageResponse_Should_ReturnExpectedValues()
    {
        // arrange
        var imageInformation = ImageInformationGenerator.Generate();

        // act
        var response = imageInformation.ToResponse();

        // assert
        response.Id.Should().Be(imageInformation.Id);
        response.Description.Should().Be(imageInformation.Description);
        response.IsProcessed.Should().Be(imageInformation.IsProcessed);
    }
}