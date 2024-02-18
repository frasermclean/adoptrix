using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Domain.Errors;

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

    [Theory]
    [InlineData("dde81f4b-e863-465f-81a4-2e7886860b81")]
    [InlineData("Beagle")]
    public async Task GetBreed_WithValidBreedIdOrName_Returns_Ok(string breedIdOrName)
    {
        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedIdOrName}");
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        ValidateBreedResponse(response!);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData(ApiFixture.UnknownBreedName)]
    public async Task GetBreed_WithUnknownBreedIdOrName_Returns_NotFound(string breedIdOrName)
    {
        // act
        var message = await httpClient.GetAsync($"/api/breeds/{breedIdOrName}");
        var response = await message.Content.ReadFromJsonAsync<MessageResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
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
        fixture.BreedsRepository
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BreedNotFoundError(breedName));
        var request = new SetBreedRequest
        {
            Name = breedName, SpeciesId = Guid.NewGuid()
        };

        // act
        var message = await httpClient.PostAsync("api/breeds", JsonContent.Create(request));
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task AddBreed_WithDuplicateBreedName_Returns_BadRequest()
    {
        var request = new SetBreedRequest
        {
            Name = "Corgi", SpeciesId = Guid.NewGuid()
        };

        // act
        var message = await httpClient.PostAsync("api/breeds", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateBreed_WithValidRequest_Should_Return_Ok()
    {
        // arrange
        var breedId = Guid.NewGuid();
        const string breedName = "Golden Retriever";
        fixture.BreedsRepository
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new BreedNotFoundError(breedName));
        var request = new SetBreedRequest
        {
            Name = breedName, SpeciesId = Guid.NewGuid()
        };

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        ValidateBreedResponse(response!);
    }

    [Fact]
    public async Task UpdateBreed_WithDuplicateBreedName_Returns_BadRequest()
    {
        // arrange
        var breedId = Guid.NewGuid();
        var request = new SetBreedRequest
        {
            Name = "Corgi", SpeciesId = Guid.NewGuid()
        };

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));
        var response = await message.Content.ReadFromJsonAsync<ValidationFailedResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = Guid.Empty;
        var request = new SetBreedRequest
        {
            Name = "Corgi", SpeciesId = Guid.NewGuid()
        };

        // act
        var message = await httpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));
        var response = await message.Content.ReadFromJsonAsync<MessageResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteBreed_WithValidBreedId_Returns_NoContent()
    {
        // arrange
        var breedId = Guid.NewGuid();

        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // act
        var message = await httpClient.DeleteAsync($"/api/breeds/{Guid.Empty}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static void ValidateBreedResponse(BreedResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.SpeciesName.Should().NotBeEmpty();
    }
}
