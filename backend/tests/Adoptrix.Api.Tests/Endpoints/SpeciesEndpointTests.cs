using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints;

public class SpeciesEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task SearchSpecies_WithValidRequest_Should_ReturnOk()
    {
        // act
        var message = await httpClient.GetAsync("api/species");
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesResponse>>();

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        responses.Should().HaveCount(3).And.AllSatisfy(response =>
        {
            response.Id.Should().NotBeEmpty();
            response.Name.Should().NotBeNullOrWhiteSpace();
        });
    }
}
