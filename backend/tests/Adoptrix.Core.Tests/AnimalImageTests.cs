namespace Adoptrix.Core.Tests;

public class AnimalImageTests
{
    [Fact]
    public void CreateAnimalImage_ShouldHaveExpectedProperties()
    {
        // arrange
        const string animalSlug = "rex-the-dog";
        const string description = "Rex in the park";
        const string originalFileName = "rex1.jpg";
        const string originalContentType = "image/jpeg";

        // act
        var image = AnimalImage.Create(animalSlug, description, originalFileName, originalContentType);

        // assert
        image.Id.Should().BeEmpty();
        image.AnimalSlug.Should().Be(animalSlug);
        image.Description.Should().Be(description);
        image.OriginalFileName.Should().Be(originalFileName);
        image.OriginalContentType.Should().Be(originalContentType);
        image.IsProcessed.Should().BeFalse();
        image.LastModifiedBy.Should().BeNull();
        image.LastModifiedUtc.Should().Be(default);
        image.OriginalBlobName.Should().Be("rex-the-dog/rex1.jpg");
        image.PreviewBlobName.Should().Be($"rex-the-dog/{image.Id}/preview.webp");
        image.ThumbnailBlobName.Should().Be($"rex-the-dog/{image.Id}/thumb.webp");
        image.FullSizeBlobName.Should().Be($"rex-the-dog/{image.Id}/full.webp");
    }
}
