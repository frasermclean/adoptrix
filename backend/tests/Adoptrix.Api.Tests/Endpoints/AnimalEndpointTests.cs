using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Animals.AddAnimal;
using Adoptrix.Api.Endpoints.Animals.DeleteAnimal;
using Adoptrix.Api.Endpoints.Animals.GetAnimal;
using Adoptrix.Api.Endpoints.Animals.SearchAnimals;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Domain;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalEndpointTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
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

    [Fact]
    public async Task AddAnimal_WithValidCommand_Should_Return_Ok()
    {
        // arrange
        var command = CreateAddAnimalCommand("Fido", "A good boy", "dog", "Labrador", Sex.Male, 2);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalCommand, AnimalResponse>(command);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Should().ContainKey("Location").WhoseValue.Should().Equal($"api/animals/{response.Id}");
        ValidateAnimalResponse(response);
    }

    [Theory]
    [InlineData("Rufus", "Another good boy", "dog", MockBreedsRepository.UnknownBreedName)]
    [InlineData("Max", "", MockSpeciesRepository.UnknownSpeciesName, "Eastern Gray")]
    [InlineData(null, null, null, null)]
    public async Task AddAnimal_WithInvalidCommand_Should_Return_BadRequest(string? name, string? description,
        string? speciesName, string? breedName, Sex? sex = default, int ageInYears = default)

    {
        // arrange
        var command = CreateAddAnimalCommand(name, description, speciesName, breedName, sex, ageInYears);

        // act
        var message = await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalCommand>(command);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("dc1ff27e-e14a-4f4d-af5a-2cba7f9c9749", HttpStatusCode.NoContent)]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.NotFound)]
    public async Task DeleteAnimal_WithValidCommand_Should_Return_Ok(Guid animalId, HttpStatusCode expectedStatusCode)
    {
        // arrange
        var command = new DeleteAnimalCommand { Id = animalId };

        // act
        var message = await httpClient.DELETEAsync<DeleteAnimalEndpoint, DeleteAnimalCommand>(command);

        // assert
        message.Should().HaveStatusCode(expectedStatusCode);
    }

    private static AddAnimalCommand CreateAddAnimalCommand(string? name, string? description, string? speciesName,
        string? breedName, Sex? sex, int ageInYears) => new()
    {
        Name = name!,
        Description = description,
        SpeciesName = speciesName!,
        BreedName = breedName!,
        Sex = sex,
        DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears))
    };

    private static void ValidateAnimalResponse(AnimalResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.SpeciesName.Should().NotBeEmpty();
        response.BreedName.Should().NotBeEmpty();
        response.Sex.Should().NotBeNull();
        response.DateOfBirth.Should().NotBe(default);
    }
}