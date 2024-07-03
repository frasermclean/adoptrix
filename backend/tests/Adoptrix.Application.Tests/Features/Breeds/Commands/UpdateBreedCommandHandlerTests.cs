using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Application.Tests.Features.Breeds.Commands;

public class UpdateBreedCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ReturnsSuccess(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        UpdateBreedRequest request, UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));

        // act
        var result = await commandHandler.Handle(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>().Which.Id.Should().Be(request.BreedId);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidBreedId_ReturnsBreedNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock, UpdateBreedRequest request,
        UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await commandHandler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidSpeciesId_ReturnsSpeciesNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        UpdateBreedRequest request,
        UpdateBreedCommandHandler commandHandler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await commandHandler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
