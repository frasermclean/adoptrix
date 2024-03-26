using Adoptrix.Api.Mapping;
using Adoptrix.Api.Tests.Generators;

namespace Adoptrix.Api.Tests.Mapping;

public class ImageResponseMapperTests
{
    [Fact]
    public void Mapping_ImageInformation_To_ImageResponse_Should_Return_ExpectedValues()
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
