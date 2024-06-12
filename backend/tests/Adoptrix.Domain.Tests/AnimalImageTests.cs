using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared.Factories;
using AutoFixture.Xunit2;

namespace Adoptrix.Domain.Tests;

public class AnimalImageTests
{
    [Fact]
    public void GetBlobName_WithInstanceMethod_ShouldReturnExpectedResult()
    {
        // arrange
        var image = AnimalImageFactory.Create();

        // act
        var blobName = image.GetBlobName(AnimalImageCategory.Original);

        // assert
        blobName.Should().Be($"{image.AnimalId}/{image.Id}/original");
    }

    [Theory]
    [InlineData(AnimalImageCategory.Original, "original")]
    [InlineData(AnimalImageCategory.Thumbnail, "thumb")]
    [InlineData(AnimalImageCategory.Preview, "preview")]
    [InlineData(AnimalImageCategory.FullSize, "full")]
    public void GetBlobName_WithValidInput_ShouldReturnExpectedResult(AnimalImageCategory category,
        string expectedSuffix)
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        // act
        var blobName = AnimalImage.GetBlobName(animalId, imageId, category);

        // assert
        blobName.Should().StartWith($"{animalId}/{imageId}/")
            .And.EndWith(expectedSuffix);
    }
}
