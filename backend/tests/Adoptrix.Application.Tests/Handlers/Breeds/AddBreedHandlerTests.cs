using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Handlers.Breeds;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Application.Tests.Handlers.Breeds;

public class AddBreedHandlerTests
{
    private readonly AddBreedHandler handler;
    private readonly Mock<IBreedsRepository> breedsRepositoryMock = new();
    private readonly Mock<ISpeciesRepository> speciesRepositoryMock = new();

    public AddBreedHandlerTests()
    {
        handler = new AddBreedHandler(breedsRepositoryMock.Object, speciesRepositoryMock.Object);
    }

    [Theory, AutoData]
    public async Task Handle_WithValidRequest_ReturnsSuccess(AddBreedRequest request)
    {
        // arrange
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.Create(request.SpeciesId));

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>();
    }

    [Theory, AutoData]
    public async Task Handle_WithInvalidSpeciesId_ReturnsSpeciesNotFoundError(AddBreedRequest request)
    {
        // arrange
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Species?)null);

        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
