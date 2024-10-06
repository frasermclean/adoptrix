using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Adoptrix.Jobs.Services;

public class ImageProcessor
{
    public const int ThumbnailWidth = 160;
    public const int PreviewHeight = 240;
    public const int FullSizeWidth = 1280;
    public const string OutputContentType = "image/webp";

    public async Task<ImageStreamBundle> ProcessOriginalAsync(Stream originalReadStream,
        CancellationToken cancellationToken)
    {
        // load original image from stream
        using var image = await Image.LoadAsync(originalReadStream, cancellationToken);

        var streams = await Task.WhenAll(
            CreateResizedImageStreamAsync(image, ThumbnailWidth, 0, cancellationToken),
            CreateResizedImageStreamAsync(image, 0, PreviewHeight, cancellationToken),
            CreateResizedImageStreamAsync(image, FullSizeWidth, 0, cancellationToken)
        );

        return new ImageStreamBundle
        {
            ThumbnailWriteStream = streams[0],
            PreviewWriteStream = streams[1],
            FullSizeWriteStream = streams[2]
        };
    }

    private static async Task<Stream> CreateResizedImageStreamAsync(Image image, int width, int height,
        CancellationToken cancellationToken)
    {
        using var clonedImage = image.Clone(context => context.Resize(width, height, KnownResamplers.Lanczos3));

        var stream = new MemoryStream();
        await clonedImage.SaveAsWebpAsync(stream, cancellationToken);
        stream.Position = 0;

        return stream;
    }
}
