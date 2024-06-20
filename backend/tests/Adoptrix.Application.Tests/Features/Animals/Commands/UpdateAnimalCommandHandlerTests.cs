using Adoptrix.Application.Errors;
using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Animals;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Features.Animals.Commands;

public class UpdateAnimalCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ShouldReturnSuccess(
        [Frozen] Mock<ILogger<UpdateAnimalCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        Animal existingAnimal, UpdateAnimalCommand command, UpdateAnimalCommandHandler commandHandler)
    {
        // arrange
        animalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAnimal);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Animal>();
        breedsRepositoryMock.Verify(repository =>
            repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()), Times.Once);
        animalsRepositoryMock.Verify(repository =>
            repository.UpdateAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()), Times.Once);
        loggerMock.VerifyLog($"Updated animal with ID: {result.Value.Id}");
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithUnknownAnimalId_ShouldReturnError(
        [Frozen] Mock<ILogger<UpdateAnimalCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        UpdateAnimalCommand command, UpdateAnimalCommandHandler commandHandler)
    {
        // arrange
        animalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().BeFailure().Which.HasError<AnimalNotFoundError>();
        loggerMock.VerifyLog($"Animal with ID {command.AnimalId} was not found", LogLevel.Error);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithUnknownBreedId_ShouldReturnError(
        [Frozen] Mock<ILogger<UpdateAnimalCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        Animal existingAnimal, UpdateAnimalCommand command, UpdateAnimalCommandHandler commandHandler)
    {
        // arrange
        animalsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAnimal);
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
        loggerMock.VerifyLog($"Breed with ID {command.BreedId} was not found", LogLevel.Error);
    }
}
