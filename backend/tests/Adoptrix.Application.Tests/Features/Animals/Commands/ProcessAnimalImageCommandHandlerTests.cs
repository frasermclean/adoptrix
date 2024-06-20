using Adoptrix.Application.Errors;
using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Support;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Features.Animals.Commands;

public class ProcessAnimalImageCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess(
        [Frozen] Mock<IAnimalImageManager> imageManagerMock,
        [Frozen] Mock<IImageProcessor> imageProcessorMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        Animal existingAnimal,
        ProcessAnimalImageCommand command, ProcessAnimalImageCommandHandler commandHandler)
    {
        // arrange
        imageManagerMock
            .Setup(manager => manager.GetImageReadStreamAsync(command.AnimalId, command.ImageId,
                It.IsAny<AnimalImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Stream.Null);
        imageProcessorMock
            .Setup(processor => processor.ProcessOriginalAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ImageStreamBundle
            {
                ThumbnailWriteStream = Stream.Null,
                PreviewWriteStream = Stream.Null,
                FullSizeWriteStream = Stream.Null
            });
        existingAnimal.Images.Add(new AnimalImage
        {
            Id = command.ImageId,
            OriginalFileName = "image123.jpg",
            OriginalContentType = "image/jpeg",
            IsProcessed = false,
            UploadedBy = Guid.NewGuid()
        });
        animalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAnimal);

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeSuccess();
        existingAnimal.Images.Single(image => image.Id == command.ImageId).IsProcessed.Should().BeTrue();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithNonExistingAnimal_ShouldReturnError(
        [Frozen] Mock<ILogger<ProcessAnimalImageCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        ProcessAnimalImageCommand command, ProcessAnimalImageCommandHandler commandHandler)
    {
        // arrange
        animalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeFailure().Which.HasError<AnimalNotFoundError>();
        loggerMock.VerifyLog($"Could not find animal with ID {command.AnimalId}, will not process command",
            LogLevel.Error);
    }
}
