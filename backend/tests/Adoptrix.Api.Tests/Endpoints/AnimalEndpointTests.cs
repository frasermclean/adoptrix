using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using System.Net.Http.Headers;
using Adoptrix.Api.Contracts.Data;
using Adoptrix.Api.Tests.Fixtures.Mocks;
using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Domain.Models;
using Microsoft.AspNetCore.Http.Extensions;
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

    [Theory, AutoData]
    public async Task SearchAnimals_WithValidRequest_ShouldReturnOk(string name, Guid breedId)
    {
        // arrange
        var query = new QueryBuilder
        {
            { "name", name },
            { "breedId", breedId.ToString() }
        };
        var uri = new Uri($"api/animals{query}", UriKind.Relative);

        // act
        var message = await httpClient.GetAsync(uri);
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<SearchAnimalsResult>>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        responses.Should().HaveCount(ApiFixture.SearchResultsCount).And.AllBeOfType<SearchAnimalsResult>();
    }

    [Theory, AutoData]
    public async Task GetAnimal_WithValidRequest_ShouldReturnOk(Guid animalId)
    {
        // act
        var message = await httpClient.GetAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);
        ValidateAnimalResponse(response!);
        fixture.AnimalsRepositoryMock.Verify(
            repository => repository.GetByIdAsync(animalId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = AnimalsRepositoryMockSetup.UnknownAnimalId;

        // act
        var message = await httpClient.GetAsync($"api/animals/{animalId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        fixture.AnimalsRepositoryMock.Verify(
            repository => repository.GetByIdAsync(animalId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAnimal_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var data = CreateSetAnimalData();

        // act
        var message = await httpClient.PostAsJsonAsync(uri, data);
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
    public async Task AddAnimal_WithInvalidRequest_ShouldReturnProblemDetails(string? name, string? description,
        Guid breedId = default, Sex sex = default, int ageInYears = default)
    {
        // arrange
        var uri = new Uri("api/admin/animals", UriKind.Relative);
        var data = CreateSetAnimalData(name!, description, breedId, sex, ageInYears);

        // act
        var message = await httpClient.PostAsJsonAsync(uri, data);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateAnimal_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);
        var data = CreateSetAnimalData();

        // act
        var message = await httpClient.PutAsJsonAsync(uri, data);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);
        response.Should().NotBeNull();
        ValidateAnimalResponse(response!);
    }

    [Fact]
    public async Task UpdateAnimal_WithInvalidRequest_ShouldReturnProblemDetails()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);
        var data = CreateSetAnimalData(name: null!);

        // act
        var message = await httpClient.PutAsJsonAsync(uri, data);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateAnimal_WithUnknownAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = AnimalsRepositoryMockSetup.UnknownAnimalId;
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);
        var data = CreateSetAnimalData();

        // act
        var message = await httpClient.PutAsJsonAsync(uri, data);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Theory, AutoData]
    public async Task DeleteAnimal_WithValidCommand_ShouldReturnNotContent(Guid animalId)
    {
        // arrange
        var uri = new Uri($"api/admin/animals/{animalId}", UriKind.Relative);

        // act
        var message = await httpClient.DeleteAsync(uri);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddAnimalImages_WithValidCommand_ShouldReturnOk()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await httpClient.PostAsync(uri, content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<AnimalResponse>(SerializerOptions);
        response.Should().NotBeNull();
        ValidateAnimalResponse(response!, 1);
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
    public async Task AddAnimalImages_WithUnknownAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = AnimalsRepositoryMockSetup.UnknownAnimalId;
        var uri = new Uri($"api/admin/animals/{animalId}/images", UriKind.Relative);
        using var content = CreateMultipartFormDataContent();

        // act
        var message = await httpClient.PostAsync(uri, content);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static SetAnimalData CreateSetAnimalData(string name = "Max", string? description = "A good boy",
        Guid? breedId = null, Sex sex = Sex.Male, int ageInYears = 2) => new(name, description,
        breedId ?? Guid.NewGuid(), sex, DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears)));

    private static void ValidateAnimalResponse(AnimalResponse response, int expectedImageCount = 0)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.SpeciesId.Should().NotBeEmpty();
        response.SpeciesName.Should().NotBeEmpty();
        response.BreedId.Should().NotBeEmpty();
        response.BreedName.Should().NotBeEmpty();
        response.Sex.Should().BeDefined();
        response.DateOfBirth.Should().NotBe(default);
        response.Images.Should().HaveCount(expectedImageCount);
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
