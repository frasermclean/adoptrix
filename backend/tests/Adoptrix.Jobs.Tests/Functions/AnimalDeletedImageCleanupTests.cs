using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Tests.Extensions;
using AutoFixture.Xunit2;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Jobs.Tests.Functions;

public class AnimalDeletedImageCleanupTests
{
    private readonly Mock<ILogger<AnimalDeletedImageCleanup>> loggerMock = new();
    private readonly Mock<IAnimalImageManager> animalImageManagerMock = new();
    private readonly AnimalDeletedImageCleanup sut;

    public AnimalDeletedImageCleanupTests()
    {
        sut = new AnimalDeletedImageCleanup(loggerMock.Object, animalImageManagerMock.Object);
    }

    [Theory, AutoData]
    public async Task Run_WithValidData_Should_Pass(int imageCount, Guid animalId)
    {
        // arrange
        var eventData = new AnimalDeletedNotification(animalId);
        animalImageManagerMock.Setup(manager => manager.DeleteAnimalImagesAsync(animalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(imageCount));

        // act
        await sut.Run(eventData);

        // assert
        loggerMock.VerifyLog($"Deleted {imageCount} images for animal {animalId}");
    }

    [Fact]
    public async Task Run_WithInvalidData_Should_Throw()
    {
        // arrange
        var notification = new AnimalDeletedNotification(Guid.NewGuid());
        animalImageManagerMock.Setup(manager => manager.DeleteAnimalImagesAsync(notification.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to delete images"));

        // act
        var act = async () => await sut.Run(notification);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }
}
