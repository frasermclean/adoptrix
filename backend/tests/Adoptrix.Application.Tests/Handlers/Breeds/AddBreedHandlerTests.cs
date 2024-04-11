using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Handlers.Breeds;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Handlers.Breeds;

public class AddBreedHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidRequest_ShouldReturnSuccess(AddBreedRequest request, AddBreedHandler handler)
    {
        // act
        var result = await handler.Handle(request);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidSpeciesId_ShouldReturnError(
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock, AddBreedRequest request, AddBreedHandler handler)
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
