using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Application.Tests.Services;

public class SlugGeneratorTests
{
    [Theory, AdoptrixAutoData]
    public async Task GenerateAsync_WithValidInput_ReturnsSlug(
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        SlugGenerator slugGenerator)
    {
        // arrange
        var (name, breed, dateOfBirth) = CreateValidInput();
        animalsRepositoryMock.Setup(repository =>
                repository.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var result = await slugGenerator.GenerateAsync(name, breed, dateOfBirth);

        // assert
        result.Should().BeSuccess().Which.Value.Should().Be("buddy-springer-spaniel-2022-01-08");
    }

    [Theory, AdoptrixAutoData]
    public async Task GenerateAsync_WithExistingAnimal_ReturnsFailure(
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        SlugGenerator slugGenerator, Animal existingAnimal)
    {
        // arrange
        var (name, breed, dateOfBirth) = CreateValidInput();
        animalsRepositoryMock.Setup(repository =>
                repository.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingAnimal);

        // act
        var result = await slugGenerator.GenerateAsync(name, breed, dateOfBirth);

        // assert
        result.Should().BeFailure().And.HaveError("Generated slug is not unique");
    }

    [Theory, AdoptrixAutoData]
    public async Task GenerateAsync_WithInvalidNames_ReturnsFailure(SlugGenerator slugGenerator)
    {
        // arrange
        var (_, breed, dateOfBirth) = CreateValidInput();
        var name = new string('a', Animal.NameMaxLength + 1);

        // act
        var result = await slugGenerator.GenerateAsync(name, breed, dateOfBirth);

        // assert
        result.Should().BeFailure().And.HaveError("Animal name is too long");
    }

    private static (string Name, Breed Breed, DateOnly DateOfBirth) CreateValidInput()
    {
        const string name = "Buddy";
        var breed = BreedFactory.Create(name: "Springer Spaniel");
        var dateOfBirth = DateOnly.Parse("2022-01-08");

        return (name, breed, dateOfBirth);
    }
}
