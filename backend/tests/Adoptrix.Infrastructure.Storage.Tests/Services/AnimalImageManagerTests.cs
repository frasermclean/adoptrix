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
    public async Task UploadImageAsync_WithValidInput_Should_ReturnSuccess(string filePath)
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        await using var imageStream = File.OpenRead(filePath);
        const string contentType = "image/jpeg";
        const ImageCategory category = ImageCategory.Original;

        // act
        var result = await animalImageManager.UploadImageAsync(animalId, imageId, imageStream, contentType, category);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task DeleteImageAsync_WithUnknownImage_Should_ReturnFailure()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        // act
        var result = await animalImageManager.DeleteImageAsync(animalId, imageId, ImageCategory.Original);

        // assert
        result.Should().BeFailure().Which.Should().HaveReason($"Blob {animalId}/{imageId}/original was not found.");
    }

    [Fact]
    public async Task DeleteImageAsync_WithExistingImage_Should_ReturnSuccess()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        await using var imageStream = File.OpenRead("Data/lab_puppy_1.jpeg");
        const string contentType = "image/jpeg";
        const ImageCategory category = ImageCategory.Original;

        // act
        await animalImageManager.UploadImageAsync(animalId, imageId, imageStream, contentType, category);
        var result = await animalImageManager.DeleteImageAsync(animalId, imageId, category);

        // assert
        result.Should().BeSuccess();
    }
}