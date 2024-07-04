using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Events;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Services;

public class AnimalImageManagerTests
{
    private readonly Mock<IAnimalsRepository> animalsRepositoryMock = new();
    private readonly Mock<IBlobContainerManager> blobContainerManagerMock = new();
    private readonly Mock<IEventPublisher> eventPublisherMock = new();
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly AnimalImageManager animalImageManager;

    public AnimalImageManagerTests()
    {
        animalImageManager = new AnimalImageManager(Mock.Of<ILogger<AnimalImageManager>>(),
            animalsRepositoryMock.Object,
            blobContainerManagerMock.Object, eventPublisherMock.Object, imageProcessorMock.Object);
    }

    [Theory, AdoptrixAutoData]
    public async Task ProcessOriginalAsync_WithValidData_ShouldReturnSuccess(Guid animalId, Guid userId,
        string fileName, string description, string contentType)
    {
        // arrange
        var stream = CreateRandomByteStream();
        blobContainerManagerMock.Setup(manager => manager.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // act
        var result =
            await animalImageManager.UploadOriginalAsync(animalId, userId, fileName, description, contentType, stream);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<AnimalImage>();
    }

    [Theory, AdoptrixAutoData]
    public async Task AddImagesToAnimalAsync_WithValidData_ShouldReturnSuccess(Animal animal, AnimalImage[] images)
    {
        // act
        var result = await animalImageManager.AddImagesToAnimalAsync(animal, images);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<AnimalResponse>();
        eventPublisherMock.Verify(publisher => publisher.PublishAsync(It.IsAny<AnimalImageAddedEvent>(),
            It.IsAny<CancellationToken>()), Times.Exactly(images.Length));
    }

    private static MemoryStream CreateRandomByteStream(int size = 1024)
    {
        var bytes = new byte[512];
        Random.Shared.NextBytes(bytes);
        return new MemoryStream(bytes);
    }

    private static (Guid animalId, Guid imageId) CreateAnimalAndImageIds()
    {
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        return (animalId, imageId);
    }
}
