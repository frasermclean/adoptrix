namespace Adoptrix.Application.Models;

public sealed class ImageStreamBundle : IAsyncDisposable
{
    public required Stream ThumbnailWriteStream { get; init; }

    public async ValueTask DisposeAsync()
    {
        await ThumbnailWriteStream.DisposeAsync();
    }
}