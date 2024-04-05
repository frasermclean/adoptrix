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
        var request = CreateSetAnimalRequest(breedId: BreedIds.GermanShepherd);

        // act
        var result = await animalsService.AddAsync(request, createdBy);

        // assert
        result.Should().BeSuccess();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be(request.Name);
        result.Value.Description.Should().Be(request.Description);
        result.Value.Breed.Id.Should().Be(request.BreedId);
        result.Value.Sex.Should().Be(request.Sex);
        result.Value.DateOfBirth.Should().Be(request.DateOfBirth);
    }

    private static SetAnimalRequest CreateSetAnimalRequest(string? name = "Max", string? description = "A good boy",
        Guid? speciesId = null, Guid? breedId = null, Sex sex = Sex.Male, int ageInYears = 2) => new()
    {
        Name = name!,
        Description = description,
        SpeciesId = speciesId ?? Guid.NewGuid(),
        BreedId = breedId ?? Guid.NewGuid(),
        Sex = sex,
        DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears))
    };
}
