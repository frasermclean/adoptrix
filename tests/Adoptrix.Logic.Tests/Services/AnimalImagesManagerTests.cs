using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Logic.Services;
using Adoptrix.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq.EntityFrameworkCore;

namespace Adoptrix.Logic.Tests.Services;

public class AnimalImagesManagerTests
{
    private readonly Mock<ILogger<AnimalImagesManager>> loggerMock = new();
    private readonly Mock<AdoptrixDbContext> dbContextMock = new(new DbContextOptions<AdoptrixDbContext>());
    private readonly Mock<IImageProcessor> imageProcessorMock = new();
    private readonly Mock<IBlobContainerManager> originalImagesContainerManagerMock = new();
    private readonly Mock<IBlobContainerManager> animalImagesContainerManagerMock = new();
    private readonly Mock<IEventPublisher> eventPublisherMock = new();
    private readonly AnimalImagesManager animalImagesManager;

    public AnimalImagesManagerTests()
    {
        animalImagesManager = new AnimalImagesManager(
            loggerMock.Object,
            dbContextMock.Object,
            imageProcessorMock.Object,
            originalImagesContainerManagerMock.Object,
            animalImagesContainerManagerMock.Object,
            eventPublisherMock.Object);
    }

    [Fact]
    public async Task DeleteImagesAsync_WithValidData_ShouldPass()
    {
        // arrange
        var animalSlug = CreateSlug();
        var data = new AnimalDeletedEvent(animalSlug);
        var blobNames = new[] { "image1.jpg", "image2.jpg", "image3.jpg" };
        animalImagesContainerManagerMock
            .Setup(manager => manager.GetBlobNamesAsync($"{animalSlug}/", It.IsAny<CancellationToken>()))
            .ReturnsAsync(blobNames);

        // act
        await animalImagesManager.DeleteImagesAsync(data);

        // assert
        animalImagesContainerManagerMock.Verify(
            manager => manager.DeleteBlobAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(blobNames.Length));
    }

    private static string CreateSlug() => Animal.CreateSlug("Bobby", new DateOnly(2022, 1, 1));
}
