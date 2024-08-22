using Adoptrix.Client.Services;

namespace Adoptrix.Web.Tests.Services;

public class ImageUrlResolverTests
{
    private readonly ImageUrlResolver imageUrlResolver = new("https://localhost:8080/images");

    [Fact]
    public void GetPreviewUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const string animalSlug = "slug";
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetPreviewUrl(animalSlug, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/slug/2/preview.webp");
    }

    [Fact]
    public void GetPreviewUrl_WithNullImageId_ShouldReturnEmptyString()
    {
        // Arrange
        const string animalSlug = "slug";

        // Act
        var url = imageUrlResolver.GetPreviewUrl(animalSlug, null);

        // Assert
        url.Should().BeEmpty();
    }

    [Fact]
    public void GetThumbUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const string animalSlug = "slug";
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetThumbUrl(animalSlug, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/slug/2/thumb.webp");
    }

    [Fact]
    public void GetFullSizeUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const string animalSlug = "slug";
        const int imageId = 2;

        // Act
        var url = imageUrlResolver.GetFullSizeUrl(animalSlug, imageId);

        // Assert
        url.Should().Be("https://localhost:8080/images/slug/2/full.webp");
    }
}
