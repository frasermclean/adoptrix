using Adoptrix.Application.Errors;
using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Application.Tests.Features.Breeds.Commands;

public class UpdateBreedCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ReturnsSuccess(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        UpdateBreedCommand command, UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(command.BreedId));

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>().Which.Id.Should().Be(command.BreedId);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidBreedId_ReturnsBreedNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock, UpdateBreedCommand command,
        UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidSpeciesId_ReturnsSpeciesNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        UpdateBreedCommand command,
        UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(command.BreedId));
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
