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
    public async Task GetBreed_WithValidBreedIdOrName_Returns_Ok(string breedIdOrName)
    {
        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedIdOrName}");
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task GetBreed_WithUnknownBreedName_Returns_NotFound()
    {
        // arrange
        const string breedName = BreedsRepositoryMockSetup.UnknownBreedName;

        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedName}");
        var details = await message.Content.ReadFromJsonAsync<ProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Fact]
    public async Task GetBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;

        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedId}");
        var details = await message.Content.ReadFromJsonAsync<ProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Fact]
    public async Task SearchBreeds_WithValidRequest_Returns_Ok()
    {
        // act
        var message = await httpClient.GetAsync("api/breeds");
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<BreedResponse>>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
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
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task AddBreed_WithDuplicateBreedName_Returns_ProblemDetails()
    {
        // arrange
        var data = CreateData();

        // act
        var message = await httpClient.PostAsync("api/breeds", JsonContent.Create(data));
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
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
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
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
        var details = await message.Content.ReadFromJsonAsync<ValidationProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        details.Should().BeOfType<ValidationProblemDetails>()
            .Which.Errors.Should().ContainKey("Name")
            .WhoseValue.Should().Contain("Breed with name: 'Corgi' already exists");
    }

    [Fact]
    public async Task UpdateBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;
        var data = CreateData(BreedsRepositoryMockSetup.UnknownBreedName);

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(data));
        var details = await message.Content.ReadFromJsonAsync<ProblemDetails>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
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
