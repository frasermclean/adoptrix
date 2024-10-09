using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(StorageEmulatorCollection))]
public class BlobContainerManagerTests(StorageEmulatorFixture fixture)
{
    private readonly IBlobContainerManager blobContainerManager = fixture.AnimalImagesBlobContainerManager;

    [Fact]
    public void Properties_WhenCalled_ReturnsValues()
    {
        // assert
        blobContainerManager.ContainerName.Should().NotBeNullOrWhiteSpace();
        blobContainerManager.ContainerUri.Should().NotBeNull();
    }

    [Fact]
    public async Task UploadBlobAsync_WhenCalled_UploadsBlob()
    {
        // arrange
        const string blobName = "test-blob";
        using var stream = CreateRandomStream();
        const string contentType = "octet/stream";

        // act
        await blobContainerManager.UploadBlobAsync(blobName, stream, contentType);

        // assert
        var blobNames = await blobContainerManager.GetBlobNamesAsync(string.Empty);
        blobNames.Should().Contain(blobName);
    }

    [Fact]
    public async Task DeleteBlobAsync_WhenCalled_DeletesBlob()
    {
        // arrange
        const string blobName = "test-blob";
        using var stream = CreateRandomStream();
        const string contentType = "octet/stream";
        await blobContainerManager.UploadBlobAsync(blobName, stream, contentType);

        // act
        await blobContainerManager.DeleteBlobAsync(blobName);

        // assert
        var blobNames = await blobContainerManager.GetBlobNamesAsync(string.Empty);
        blobNames.Should().NotContain(blobName);
    }

    [Fact]
    public async Task OpenReadStreamAsync_WhenCalled_ReturnsStream()
    {
        // arrange
        const int streamLength = 2048;
        const string blobName = "test-blob";
        using var stream = CreateRandomStream(streamLength);
        const string contentType = "octet/stream";
        await blobContainerManager.UploadBlobAsync(blobName, stream, contentType);

        // act
        await using var readStream = await blobContainerManager.OpenReadStreamAsync(blobName);

        // assert
        readStream.Should().NotBeNull();
        readStream.Length.Should().Be(streamLength);
        readStream.CanRead.Should().BeTrue();
    }

    private static MemoryStream CreateRandomStream(int size = 1024)
    {
        var buffer = new byte[size];
        Random.Shared.NextBytes(buffer);
        return new MemoryStream(buffer);
    }
}
