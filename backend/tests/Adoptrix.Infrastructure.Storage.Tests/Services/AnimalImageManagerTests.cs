﻿using Adoptrix.Infrastructure.Storage.Services;
using Adoptrix.Infrastructure.Storage.Tests.Fixtures;
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
        var (animalId, imageId) = CreateAnimalAndImageIds();
        await using var imageStream = File.OpenRead(filePath);
        const string contentType = "image/jpeg";

        // act
        var result = await animalImageManager.UploadImageAsync(animalId, imageId, imageStream, contentType);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task DeleteImageAsync_WithUnknownImage_Should_ReturnFailure()
    {
        // arrange
        var (animalId, imageId) = CreateAnimalAndImageIds();

        // act
        var result = await animalImageManager.DeleteImageAsync(animalId, imageId);

        // assert
        result.Should().BeFailure().Which.Should().HaveReason($"Blob {animalId}/{imageId}/original was not found.");
    }

    [Fact]
    public async Task DeleteImageAsync_WithExistingImage_Should_ReturnSuccess()
    {
        // arrange
        var (animalId, imageId) = CreateAnimalAndImageIds();
        await using var imageStream = File.OpenRead("Data/lab_puppy_1.jpeg");
        const string contentType = "image/jpeg";

        // act
        await animalImageManager.UploadImageAsync(animalId, imageId, imageStream, contentType);
        var result = await animalImageManager.DeleteImageAsync(animalId, imageId);

        // assert
        result.Should().BeSuccess();
    }

    [Fact]
    public async Task DeleteAnimalImagesAsync_WithExistingImages_Should_ReturnSuccess()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId1 = Guid.NewGuid();
        var imageId2 = Guid.NewGuid();
        var imageId3 = Guid.NewGuid();
        await using var imageStream1 = File.OpenRead("Data/lab_puppy_1.jpeg");
        await using var imageStream2 = File.OpenRead("Data/lab_puppy_2.jpeg");
        await using var imageStream3 = File.OpenRead("Data/lab_puppy_3.jpeg");
        const string contentType = "image/jpeg";

        // act
        var uploadResult1 =
            await animalImageManager.UploadImageAsync(animalId, imageId1, imageStream1, contentType);
        var uploadResult2 =
            await animalImageManager.UploadImageAsync(animalId, imageId2, imageStream2, contentType);
        var uploadResult3 =
            await animalImageManager.UploadImageAsync(animalId, imageId3, imageStream3, contentType);
        var deleteResult = await animalImageManager.DeleteAnimalImagesAsync(animalId);

        // assert
        uploadResult1.Should().BeSuccess();
        uploadResult2.Should().BeSuccess();
        uploadResult3.Should().BeSuccess();
        deleteResult.Should().BeSuccess().Which.Should().HaveValue(3);
    }

    private static (Guid animalId, Guid imageId) CreateAnimalAndImageIds()
    {
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        return (animalId, imageId);
    }
}