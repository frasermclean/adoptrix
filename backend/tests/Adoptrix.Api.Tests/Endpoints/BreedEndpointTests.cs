using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

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
        var response = await message.Content.ReadFromJsonAsync<NotFoundResponse>(SerializerOptions);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Should().NotBeNull();
    }

    private static void ValidateBreedResponse(BreedResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.SpeciesName.Should().NotBeEmpty();
    }
}
