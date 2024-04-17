using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Features.Animals.Validators;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Application.Tests.Features.Animals.Validators;

public class AddAnimalCommandValidatorTests
{
    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithValidCommand_ShouldBeValid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        Breed existingBreed,
        AddAnimalCommandValidator validator, AddAnimalCommand command)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingBreed);

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithUnknownBreedId_ShouldBeInvalid(
        [Frozen] Mock<IBreedsRepository> breedsRepositoryMock,
        AddAnimalCommandValidator validator, AddAnimalCommand command)
    {
        // arrange
        breedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(command.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(command.BreedId))
            .Which.ErrorMessage.Should().Be($"Could not find breed with ID: '{command.BreedId}'");
    }

    [Theory, AdoptrixAutoData]
    public async Task ValidateAsync_WithEmptyName_ShouldBeInvalid(
        AddAnimalCommandValidator validator, AddAnimalCommand command)
    {
        // arrange
        command = command with { Name = string.Empty };

        // act
        var result = await validator.ValidateAsync(command);

        // assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(failure => failure.PropertyName == nameof(command.Name))
            .Which.ErrorMessage.Should().Be("'Name' must not be empty.");
    }
}
