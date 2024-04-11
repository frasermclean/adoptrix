﻿using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Handlers.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Handlers.Animals;

public class AddAnimalHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ShouldReturnSuccess(
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddAnimalRequest request, AddAnimalHandler handler)
    {
        // act
        var result = await handler.Handle(request, CancellationToken.None);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Animal>();
        breedsRepositoryMock.Verify(repository =>
            repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()), Times.Once);
        animalsRepositoryMock.Verify(repository =>
            repository.AddAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WhenBreedIsInvalid_ShouldReturnError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddAnimalRequest request, AddAnimalHandler handler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await handler.Handle(request, CancellationToken.None);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }
}