using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Features.Animals.Commands;

public class CleanupAnimalImagesCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess(
        [Frozen] Mock<IAnimalImageManager> imageManagerMock,
        CleanupAnimalImagesCommand command, CleanupAnimalImagesCommandHandler commandHandler)
    {
        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeSuccess();
        imageManagerMock.Verify(manager => manager.DeleteAnimalImagesAsync(command.AnimalId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithFailedDelete_ShouldReturnError(
        [Frozen] Mock<ILogger<CleanupAnimalImagesCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalImageManager> imageManagerMock,
        CleanupAnimalImagesCommand command, CleanupAnimalImagesCommandHandler commandHandler)
    {
        // arrange
        imageManagerMock
            .Setup(manager => manager.DeleteAnimalImagesAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to delete images"));

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeFailure();
        loggerMock.VerifyLog($"Failed to delete images for animal {command.AnimalId}", LogLevel.Error);
    }
}
