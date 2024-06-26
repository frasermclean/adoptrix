﻿using Adoptrix.Application.Errors;
using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Extensions;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Tests.Features.Animals.Commands;

public class AddAnimalCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ShouldReturnSuccess(
        [Frozen] Mock<ILogger<AddAnimalCommandHandler>> loggerMock,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddAnimalCommand command, AddAnimalCommandHandler commandHandler)
    {
        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Animal>();
        breedsRepositoryMock.Verify(repository =>
            repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()), Times.Once);
        animalsRepositoryMock.Verify(repository =>
            repository.AddAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()), Times.Once);
        loggerMock.VerifyLog($"Animal with ID {result.Value.Id} was added successfully");
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WhenBreedIsInvalid_ShouldReturnError(
        [Frozen] Mock<ILogger<AddAnimalCommandHandler>> loggerMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddAnimalCommand command, AddAnimalCommandHandler commandHandler)
    {
        // arrange
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
