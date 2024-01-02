using Adoptrix.Application.Services;
using SixLabors.ImageSharp;

namespace Adoptrix.Application.Tests.Services;

public class ImageProcessorTests
{
    private readonly ImageProcessor imageProcessor = new();

    [Theory]
    [InlineData("Data/lab_puppy_1.jpeg")]
    [InlineData("Data/lab_puppy_2.jpeg")]
    [InlineData("Data/lab_puppy_3.jpeg")]
    public async Task ProcessImageAsync_WhenCalledWithValidStream_ThenCreatesThumbnail(string filePath)
    {
        // arrange
        var readStream = File.OpenRead(filePath);

        // act
        await using var bundle = await imageProcessor.ProcessOriginalAsync(readStream, default);
        using var thumbnailImage = await Image.LoadAsync(bundle.ThumbnailWriteStream);
        using var previewImage = await Image.LoadAsync(bundle.PreviewWriteStream);
        using var fullSizeImage = await Image.LoadAsync(bundle.FullSizeWriteStream);

        // assert
        thumbnailImage.Width.Should().Be(ImageProcessor.ThumbnailWidth);
        thumbnailImage.Metadata.DecodedImageFormat?.DefaultMimeType.Should().Be(ImageProcessor.OutputContentType);
        previewImage.Height.Should().Be(ImageProcessor.PreviewHeight);
        previewImage.Metadata.DecodedImageFormat?.DefaultMimeType.Should().Be(ImageProcessor.OutputContentType);
        fullSizeImage.Width.Should().Be(ImageProcessor.FullSizeWidth);
        fullSizeImage.Metadata.DecodedImageFormat?.DefaultMimeType.Should().Be(ImageProcessor.OutputContentType);
    }
}