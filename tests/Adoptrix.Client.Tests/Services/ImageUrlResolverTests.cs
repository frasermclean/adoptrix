using Adoptrix.Client.Services;

namespace Adoptrix.Web.Tests.Services;

public class ImageUrlResolverTests
{
    private readonly ImageUrlResolver imageUrlResolver = new("https://localhost:8080/images");

    [Fact]
    public void GetPreviewUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const int animalId = 1;
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetPreviewUrl(animalId, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/1/2/preview.webp");
    }

    [Fact]
    public void GetPreviewUrl_WithNullImageId_ShouldReturnEmptyString()
    {
        // Arrange
        const int animalId = 1;

        // Act
        var url = imageUrlResolver.GetPreviewUrl(animalId, null);

        // Assert
        url.Should().BeEmpty();
    }

    [Fact]
    public void GetThumbUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const int animalId = 1;
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetThumbUrl(animalId, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/1/2/thumb.webp");
    }

    [Fact]
    public void GetFullSizeUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const int animalId = 1;
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetFullSizeUrl(animalId, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/1/2/full.webp");
    }
}
