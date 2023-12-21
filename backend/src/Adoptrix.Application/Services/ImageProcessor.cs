using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Adoptrix.Application.Services;

public interface IImageProcessor
{
    void CreateThumbnail(Stream stream);
}

public class ImageProcessor : IImageProcessor
{
    private const int ThumbnailWidth = 200;
    private const int ThumbnailHeight = 160;

    public void CreateThumbnail(Stream stream)
    {
        var image = Image.Load(stream);
        image.Mutate(context => context.Resize(ThumbnailWidth, ThumbnailHeight));
    }
}