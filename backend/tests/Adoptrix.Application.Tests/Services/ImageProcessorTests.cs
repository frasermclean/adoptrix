using Adoptrix.Application.Services;
using SixLabors.ImageSharp;

namespace Adoptrix.Application.Tests.Services;

public class ImageProcessorTests
{
    private readonly ImageProcessor imageProcessor = new();

    [Theory]
    [InlineData("Resources/lab_puppy_1.jpeg")]
    [InlineData("Resources/lab_puppy_2.jpeg")]
    [InlineData("Resources/lab_puppy_3.jpeg")]
    public async Task ProcessImageAsync_WhenCalledWithValidStream_ThenCreatesThumbnail(string filePath)
    {
        // arrange
        var readStream = File.OpenRead(filePath);

        // act
        await using var bundle = await imageProcessor.ProcessOriginalAsync(readStream, default);
        using var thumbnail = await Image.LoadAsync(bundle.ThumbnailWriteStream);

        // assert
        thumbnail.Width.Should().Be(ImageProcessor.ThumbnailWidth);
        thumbnail.Metadata.DecodedImageFormat?.DefaultMimeType.Should().Be(ImageProcessor.OutputContentType);
    }
}