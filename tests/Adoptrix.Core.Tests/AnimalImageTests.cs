namespace Adoptrix.Core.Tests;

public class AnimalImageTests
{
    [Fact]
    public void NewAnimalImage_ShouldHaveExpectedProperties()
    {
        // act
        var image = new AnimalImage
        {
            Id = Guid.NewGuid(),
            AnimalSlug = "rex-the-dog",
            Description = "Rex in the park",
            OriginalFileName = "rex1.jpg",
            OriginalContentType = "image/jpeg",
            IsProcessed = false,
            LastModifiedBy = Guid.NewGuid(),
            LastModifiedUtc = DateTime.UtcNow
        };

        // assert
        image.Id.Should().NotBeEmpty();
        image.AnimalSlug.Should().Be("rex-the-dog");
        image.Description.Should().Be("Rex in the park");
        image.OriginalFileName.Should().Be("rex1.jpg");
        image.OriginalContentType.Should().Be("image/jpeg");
        image.IsProcessed.Should().BeFalse();
        image.LastModifiedBy.Should().NotBeEmpty();
        image.LastModifiedUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        image.OriginalBlobName.Should().Be("rex-the-dog/rex1.jpg");
        image.PreviewBlobName.Should().Be($"rex-the-dog/{image.Id}/preview.webp");
        image.ThumbnailBlobName.Should().Be($"rex-the-dog/{image.Id}/thumb.webp");
        image.FullSizeBlobName.Should().Be($"rex-the-dog/{image.Id}/full.webp");
    }
}
