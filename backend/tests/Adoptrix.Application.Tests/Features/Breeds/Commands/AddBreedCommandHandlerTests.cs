using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Features.Breeds.Commands;

public class AddBreedCommandHandlerTests
{
    [Theory, AdoptrixAutoData]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess(AddBreedCommand command, AddBreedCommandHandler commandHandler)
    {
        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeSuccess().Which.Value.Should().BeOfType<Breed>();
    }

    [Theory, AdoptrixAutoData]
    public async Task Handle_WithInvalidSpeciesId_ShouldReturnError(
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock, AddBreedCommand command, AddBreedCommandHandler commandHandler)
    {
        // arrange
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Species?)null);

        // act
        var result = await commandHandler.Handle(command);

        // assert
        result.Should().BeFailure().Which.HasError<SpeciesNotFoundError>();
    }
}
