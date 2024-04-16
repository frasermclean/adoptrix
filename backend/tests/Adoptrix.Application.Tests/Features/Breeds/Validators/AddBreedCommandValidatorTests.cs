using Adoptrix.Application.Features.Breeds.Commands;
using Adoptrix.Application.Features.Breeds.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Features.Breeds.Validators;

public class AddBreedCommandValidatorTests
{
    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithValidCommand_ShouldBeValid(
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        Species existingSpecies,
        AddBreedCommandValidator validator, AddBreedCommand command)
    {
        // arrange
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSpecies);
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithExistingBreedName_ShouldBeInvalid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddBreedCommandValidator validator, AddBreedCommand command, Breed existingBreed)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBreed);

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(command.Name))
            .Which.ErrorMessage.Should().Be($"Breed with name: '{command.Name}' already exists");
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithUnknownSpeciesId_ShouldBeInvalid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        [Frozen] Mock<ISpeciesRepository> speciesRepositoryMock,
        AddBreedCommandValidator validator, AddBreedCommand command)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);
        speciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(command.SpeciesId))
            .Which.ErrorMessage.Should().Be($"Could not find species with ID: '{command.SpeciesId}'");
    }
}
