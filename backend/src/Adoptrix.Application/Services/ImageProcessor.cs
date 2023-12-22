using Adoptrix.Application.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Adoptrix.Application.Services;

public interface IImageProcessor
{
    Task<ImageStreamBundle> ProcessOriginalAsync(Stream originalReadStream,
        CancellationToken cancellationToken = default);
}

public class ImageProcessor : IImageProcessor
{
    public const int ThumbnailWidth = 160;
    public const string OutputContentType = "image/webp";

    public async Task<ImageStreamBundle> ProcessOriginalAsync(Stream originalReadStream,
        CancellationToken cancellationToken)
    {
        // load original image from stream
        using var image = await Image.LoadAsync(originalReadStream, cancellationToken);

        return new ImageStreamBundle
        {
            ThumbnailWriteStream = await CreateThumbnailStreamAsync(image, cancellationToken)
        };
    }

    private static async Task<Stream> CreateThumbnailStreamAsync(Image image, CancellationToken cancellationToken)
    {
        image.Mutate(context => context.Resize(ThumbnailWidth, 0));
        var stream = new MemoryStream();
        await image.SaveAsWebpAsync(stream, cancellationToken);
        stream.Position = 0;

        return stream;
    }
}