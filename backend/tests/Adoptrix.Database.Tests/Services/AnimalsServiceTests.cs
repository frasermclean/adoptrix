using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Extensions;
using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class AnimalsServiceTests(DatabaseFixture fixture)
{
    private readonly IAnimalsService animalsService = fixture.AnimalsRepository!;

    [Fact]
    public async Task GetAsync_WithInvalidId_ShouldFail()
    {
        // arrange
        var animalId = Guid.Empty;

        // act
        var result = await animalsService.GetAsync(animalId);

        // assert
        result.Should().BeFailure().Which
            .Should().HaveReason<AnimalNotFoundError>($"Could not find animal with ID {animalId}");
    }

    [Fact]
    public async Task AddAsync_WithValidRequest_ShouldPass()
    {
        // arrange
        var createdBy = Guid.NewGuid();
        var request = CreateSetAnimalRequest(breedId: BreedIds.GermanShepherd, userId: createdBy);

        // act
        var result = await animalsService.AddAsync(request);

        // assert
        result.Should().BeSuccess();
        var animal = result.Value;
        animal.Id.Should().NotBeEmpty();
        animal.Name.Should().Be(request.Name);
        animal.Description.Should().Be(request.Description);
        animal.Breed.Id.Should().Be(request.BreedId);
        animal.Sex.Should().Be(request.Sex);
        animal.DateOfBirth.Should().Be(request.DateOfBirth);
        animal.CreatedBy.Should().Be(createdBy);
    }

    private static SetAnimalRequest CreateSetAnimalRequest(string name = "Max", string? description = "A good boy",
        Guid? speciesId = null, Guid? breedId = null, Sex sex = Sex.Male, int ageInYears = 2, Guid? userId = null) =>
        new()
        {
            Name = name,
            Description = description,
            SpeciesId = speciesId ?? Guid.NewGuid(),
            BreedId = breedId ?? Guid.NewGuid(),
            Sex = sex,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears)),
            UserId = userId ?? Guid.Empty
        };
}
