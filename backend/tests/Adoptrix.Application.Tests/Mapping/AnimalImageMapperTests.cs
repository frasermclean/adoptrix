using Adoptrix.Tests.Shared.Factories;
using AnimalImageMapper = Adoptrix.Application.Mapping.AnimalImageMapper;

namespace Adoptrix.Application.Tests.Mapping;

public class AnimalImageMapperTests
{
    [Fact]
    public void Mapping_ImageInformation_To_ImageResponse_Should_Return_ExpectedValues()
    {
        // arrange
        var animalImage = AnimalImageFactory.Create();

        // act
        var response = AnimalImageMapper.ToResponse(animalImage);

        // assert
        response.Id.Should().Be(animalImage.Id);
        response.Description.Should().Be(animalImage.Description);
        response.IsProcessed.Should().Be(animalImage.IsProcessed);
    }
}
