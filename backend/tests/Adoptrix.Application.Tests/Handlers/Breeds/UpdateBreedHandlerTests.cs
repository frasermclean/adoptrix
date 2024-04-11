using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Handlers.Breeds;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Application.Tests.Handlers.Breeds;

public class UpdateBreedHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ReturnsSuccess(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        UpdateBreedRequest request, UpdateBreedHandler handler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>().Which.Id.Should().Be(request.BreedId);
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidBreedId_ReturnsBreedNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock, UpdateBreedRequest request,
        UpdateBreedHandler handler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<BreedNotFoundError>();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidSpeciesId_ReturnsSpeciesNotFoundError(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        UpdateBreedRequest request,
        UpdateBreedHandler handler)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
