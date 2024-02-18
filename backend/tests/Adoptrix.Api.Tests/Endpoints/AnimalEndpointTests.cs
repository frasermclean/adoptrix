using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Domain;
using System.Net.Http.Headers;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    [Fact]
    public async Task SearchAnimals_WithValidRequest_Should_ReturnOk()
    {
        // act
        var message = await httpClient.GetAsync("api/animals");
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<AnimalResponse>>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        responses.Should().HaveCount(ApiFixture.SearchResultsCount).And.AllSatisfy(ValidateAnimalResponse);
    }

    [Theory]
    [InlineData("dc1ff27e-e14a-4f4d-af5a-2cba7f9c9749", HttpStatusCode.OK)]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.NotFound)]
    public async Task GetAnimal_WithValidRequest_Should_ExpectedStatusCode(Guid animalId,
        HttpStatusCode expectedStatusCode)
    {
        // act
        var message = await httpClient.GetAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(expectedStatusCode);
        fixture.AnimalsRepository.Verify(repository => repository.GetAsync(animalId, It.IsAny<CancellationToken>()),
            Times.Once);
        if (expectedStatusCode == HttpStatusCode.OK)
        {
            var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);
            ValidateAnimalResponse(response!);
        }
    }

    [Fact]
    public async Task AddAnimal_WithValidCommand_Should_Return_Ok()
    {
        // arrange
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var request = CreateAddAnimalRequest();

        // act
        var message = await httpClient.PostAsJsonAsync(uri, request);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        response.Should().NotBeNull();
        message.Headers.Should().ContainKey("Location").WhoseValue.Should().Equal($"/api/animals/{response!.Id}");
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
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var request = CreateAddAnimalRequest(name, description, speciesName, breedName, sex, ageInYears);

        // act
        var message = await httpClient.PostAsJsonAsync(uri, request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("dc1ff27e-e14a-4f4d-af5a-2cba7f9c9749", HttpStatusCode.NoContent)]
    [InlineData("00000000-0000-0000-0000-000000000000", HttpStatusCode.NotFound)]
    public async Task DeleteAnimal_WithValidCommand_Should_Return_Ok(Guid animalId, HttpStatusCode expectedStatusCode)
    {
        // arrange
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);

        // act
        var message = await httpClient.DeleteAsync(uri);

        // assert
        message.Should().HaveStatusCode(expectedStatusCode);
    }

    [Theory]
    [InlineData]
    public async Task AddAnimalImages_WithValidCommand_Should_Return_Ok(string fileName = "lab_puppy_1.jpeg")
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);

        using var fileContent = new StreamContent(File.OpenRead("Data/lab_puppy_1.jpeg"));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        var content = new MultipartFormDataContent
        {
            {
                fileContent, "First image", fileName
            }
        };

        // act
        var message = await httpClient.PostAsync(uri, content);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Should().NotBeNull();
        ValidateAnimalResponse(response!);
    }

    private static AddAnimalRequest CreateAddAnimalRequest(string? name = "Max", string? description = "A good boy",
        string? speciesName = "dog", string? breedName = "Labrador", Sex? sex = Sex.Male, int ageInYears = 2) => new()
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
