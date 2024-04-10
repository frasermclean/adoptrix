using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Handlers.Breeds;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Factories;

namespace Adoptrix.Application.Tests.Handlers.Breeds;

public class UpdateBreedHandlerTests
{
    private readonly UpdateBreedHandler handler;
    private readonly Mock<IBreedsRepository> breedsRepositoryMock = new();
    private readonly Mock<ISpeciesRepository> speciesRepositoryMock = new();

    public UpdateBreedHandlerTests()
    {
        handler = new UpdateBreedHandler(breedsRepositoryMock.Object, speciesRepositoryMock.Object);
    }

    [Theory, AutoData]
    public async Task Handle_WithValidRequest_ReturnsSuccess(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.Create(request.SpeciesId));

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>();
    }

    [Theory, AutoData]
    public async Task Handle_WithInvalidBreedId_ReturnsBreedNotFoundError(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Breed?)null);

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory, AutoData]
    public async Task Handle_WithInvalidSpeciesId_ReturnsSpeciesNotFoundError(UpdateBreedRequest request)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Species?)null);

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
