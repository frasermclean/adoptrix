namespace Adoptrix.Application;

public sealed class ImageStreamBundle : IAsyncDisposable
{
    public required Stream ThumbnailWriteStream { get; init; }
    public required Stream PreviewWriteStream { get; init; }
    public required Stream FullSizeWriteStream { get; init; }

    public async ValueTask DisposeAsync()
    {
        await ThumbnailWriteStream.DisposeAsync();
        await PreviewWriteStream.DisposeAsync();
        await FullSizeWriteStream.DisposeAsync();
    }
}
