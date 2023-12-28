using Adoptrix.Domain;
using Adoptrix.Infrastructure.Storage.Services;
using Adoptrix.Infrastructure.Storage.Tests.Fixtures;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Storage.Tests.Services;

[Trait("Category", "Integration")]
public class AnimalImageManagerTests(StorageEmulatorFixture fixture) : IClassFixture<StorageEmulatorFixture>
{
    private readonly AnimalImageManager animalImageManager = new(Mock.Of<ILogger<AnimalImageManager>>(),
        fixture.BlobContainerClient!);

    [Theory]
    [InlineData("Data/lab_puppy_1.jpeg")]
    [InlineData("Data/lab_puppy_2.jpeg")]
    [InlineData("Data/lab_puppy_3.jpeg")]
    public async Task UploadImageAsync_WhenCalled_ReturnsSuccess(string filePath)
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        await using var imageStream = File.OpenRead(filePath);
        const string contentType = "image/jpeg";
        const ImageCategory category = ImageCategory.Original;
        var cancellationToken = CancellationToken.None;

        // act
        var result = await animalImageManager.UploadImageAsync(animalId, imageId, imageStream, contentType, category,
            cancellationToken);

        // assert
        result.Should().BeSuccess();
    }
}