using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Domain;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

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
    public async Task AddAnimal_WithValidRequest_Should_Return_Ok()
    {
        // arrange
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var request = CreateSetAnimalRequest();

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
    [InlineData("Rufus", "Another good boy")]
    [InlineData("Max", "")]
    [InlineData(null, null)]
    public async Task AddAnimal_WithInvalidRequest_Should_Return_ProblemDetails(string? name, string? description,
        Guid speciesId = default, Guid breedId = default, Sex sex = default, int ageInYears = default)
    {
        // arrange
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var request = CreateSetAnimalRequest(name, description, speciesId, breedId, sex, ageInYears);

        // act
        var message = await httpClient.PostAsJsonAsync(uri, request);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateAnimal_WithValidRequest_Should_Return_Ok()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);
        var request = CreateSetAnimalRequest();

        // act
        var message = await httpClient.PutAsJsonAsync(uri, request);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Should().NotBeNull();
        ValidateAnimalResponse(response!);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidRequest_Should_Return_ProblemDetails()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);
        var request = CreateSetAnimalRequest(name: null);

        // act
        var message = await httpClient.PutAsJsonAsync(uri, request);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateAnimal_WithUnknownAnimalId_Should_Return_NotFound()
    {
        // arrange
        var uri = new Uri($"api/admin/animals/{Guid.Empty}", UriKind.Relative);
        var request = CreateSetAnimalRequest();

        // act
        var message = await httpClient.PutAsJsonAsync(uri, request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
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

    [Fact]
    public async Task AddAnimalImages_WithValidCommand_Should_Return_Ok()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await httpClient.PostAsync(uri, content);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Should().NotBeNull();
        ValidateAnimalResponse(response!);
    }

    [Fact]
    public async Task AddAnimalImages_WithInvalidContent_Should_Return_ProblemDetails()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);
        using var content = CreateMultipartFormDataContent(contentType: "application/json");

        // act
        var message = await httpClient.PostAsync(uri, content);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddAnimalImages_WithUnknownAnimalId_Should_Return_NotFound()
    {
        // arrange
        var animalId = Guid.Empty;
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await httpClient.PostAsync(uri, content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
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

    private static void ValidateAnimalResponse(AnimalResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.SpeciesId.Should().NotBeEmpty();
        response.BreedId.Should().NotBeEmpty();
        response.Sex.Should().NotBeNull();
        response.DateOfBirth.Should().NotBe(default);
    }

    private static MultipartFormDataContent CreateMultipartFormDataContent(string fileName = "lab_puppy_1.jpeg",
        string contentName = "First image", string contentType = "image/jpeg")
    {
        var content = new StreamContent(File.OpenRead($"Data/{fileName}"));
        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

        return new MultipartFormDataContent
        {
            {
                content, contentName, fileName
            }
        };
    }
}
