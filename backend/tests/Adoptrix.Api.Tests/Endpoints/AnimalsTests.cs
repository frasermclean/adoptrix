using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Animals.AddAnimal;
using Adoptrix.Api.Endpoints.Animals.GetAnimal;
using Adoptrix.Api.Endpoints.Animals.SearchAnimals;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Domain;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalsTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    : TestClass<ApiTestFixture>(fixture, outputHelper)
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task SearchAnimals_WithValidRequest_Should_ReturnOk()
    {
        var command = new SearchAnimalsCommand();

        // act
        var (message, responses) =
            await httpClient
                .GETAsync<SearchAnimalsEndpoint, SearchAnimalsCommand, IEnumerable<AnimalResponse>>(command);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        responses.Should().HaveCount(3).And.AllSatisfy(ValidateAnimalResponse);
    }

    [Theory]
    [InlineData("dc1ff27e-e14a-4f4d-af5a-2cba7f9c9749", HttpStatusCode.OK)]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.NotFound)]
    public async Task GetAnimal_WithValidRequest_Should_ExpectedStatusCode(Guid animalId,
        HttpStatusCode expectedStatusCode)
    {
        // arrange
        var command = new GetAnimalCommand { Id = animalId };

        // act
        var (message, response) =
            await httpClient.GETAsync<GetAnimalEndpoint, GetAnimalCommand, AnimalResponse>(command);

        // assert
        message.Should().HaveStatusCode(expectedStatusCode);
        if (expectedStatusCode == HttpStatusCode.OK)
        {
            ValidateAnimalResponse(response);
        }
    }

    [Theory]
    [InlineData("Fido", "A good boy", "dog", "Labrador", Sex.Male, 2, HttpStatusCode.Created)]
    [InlineData("Rufus", "Another good boy", "dog", MockBreedsRepository.UnknownBreedName)]
    [InlineData("Max", "", MockSpeciesRepository.UnknownSpeciesName, "Eastern Gray")]
    [InlineData(null, null, null, null)]
    public async Task AddAnimal_Should_Return_ExpectedResult(string? name, string? description, string? species,
        string? breed, Sex? sex = default, int ageInYears = default,
        HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest)
    {
        // arrange
        var command = new AddAnimalCommand
        {
            Name = name!,
            Description = description,
            Species = species!,
            Breed = breed!,
            Sex = sex,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears))
        };

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalCommand, AnimalResponse>(command);

        // assert
        message.Should().HaveStatusCode(expectedStatusCode);
        if (expectedStatusCode == HttpStatusCode.Created)
        {
            message.Headers.Should().ContainKey("Location").WhoseValue.Should().Equal($"api/animals/{response.Id}");
            ValidateAnimalResponse(response);
        }
    }

    private static void ValidateAnimalResponse(AnimalResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.Species.Should().NotBeEmpty();
        response.Breed.Should().NotBeEmpty();
        response.Sex.Should().NotBeNull();
        response.DateOfBirth.Should().NotBe(default);
    }
}