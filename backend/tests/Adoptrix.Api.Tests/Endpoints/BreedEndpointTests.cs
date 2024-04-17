using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Api.Tests.Fixtures.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Tests.Endpoints;

public class BreedEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
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
    public async Task GetBreed_WithValidBreedId_Returns_Ok(Guid breedId)
    {
        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task GetBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;

        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        var details = await message.Content.ReadFromJsonAsync<ProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Fact]
    public async Task SearchBreeds_WithValidRequest_Returns_Ok()
    {
        // act
        var message = await httpClient.GetAsync("api/breeds");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<BreedResponse>>(SerializerOptions);
        responses.Should().HaveCount(ApiFixture.SearchResultsCount).And.AllSatisfy(ValidateBreedResponse);
    }

    [Fact]
    public async Task AddBreed_WithValidRequest_Returns_Created()
    {
        // arrange
        const string breedName = "Sausage Dog";
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => null);
        var data = new SetBreedRequest(breedName, Guid.NewGuid());

        // act
        var message = await httpClient.PostAsync("api/breeds", JsonContent.Create(data));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task AddBreed_WithDuplicateBreedName_Returns_ProblemDetails()
    {
        // arrange
        var data = CreateData();

        // act
        var message = await httpClient.PostAsync("api/breeds", JsonContent.Create(data));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateBreed_WithValidData_Should_Return_Ok()
    {
        // arrange
        var breedId = Guid.NewGuid();
        const string breedName = "Golden Retriever";
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => null);
        var data = CreateData(breedName);

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(data));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task UpdateBreed_WithDuplicateBreedName_Returns_ProblemDetails()
    {
        // arrange
        var breedId = Guid.NewGuid();
        var data = CreateData();

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(data));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().ContainKey("Name");
    }

    [Fact]
    public async Task UpdateBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;
        var data = CreateData();

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(data));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        var details = await message.Content.ReadFromJsonAsync<ProblemDetails>(SerializerOptions);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Theory, AutoData]
    public async Task DeleteBreed_WithValidBreedId_Returns_NoContent(Guid breedId)
    {
        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;

        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static SetBreedRequest CreateData(string name = "Corgi", Guid? speciesId = null)
        => new(name, speciesId ?? Guid.NewGuid());

    private static void ValidateBreedResponse(BreedResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.SpeciesId.Should().NotBeEmpty();
    }
}
