using Adoptrix.Client.Services;

namespace Adoptrix.Client.Tests.Services;

public class ImageUrlResolverTests
{
    private const string BaseUrl = "https://localhost:8080/images";
    private readonly ImageUrlResolver imageUrlResolver = new(BaseUrl);

    [Fact]
    public void GetPreviewUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const string animalSlug = "slug";
        var imageId = Guid.NewGuid();

        // Act
        var url = imageUrlResolver.GetPreviewUrl(animalSlug, imageId);

        // Assert
        url.Should().Be($"{BaseUrl}/{animalSlug}/{imageId}/preview.webp");
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
        var imageId = Guid.NewGuid();

        // Act
        var url = imageUrlResolver.GetThumbUrl(animalSlug, imageId);

        // Assert
        url.Should().Be($"{BaseUrl}/{animalSlug}/{imageId}/thumb.webp");
    }

    [Fact]
    public void GetFullSizeUrl_WithValidParameters_ShouldReturnCorrectUrl()
    {
        // Arrange
        const string animalSlug = "slug";
        var imageId = Guid.NewGuid();

        // Act
        var url = imageUrlResolver.GetFullSizeUrl(animalSlug, imageId);

        // Assert
        url.Should().Be($"{BaseUrl}/{animalSlug}/{imageId}/full.webp");
    }
}
