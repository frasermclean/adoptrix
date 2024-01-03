using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Species.SearchSpecies;
using Adoptrix.Application.Commands.Species;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class SpeciesEndpointTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    : TestClass<ApiTestFixture>(fixture, outputHelper)
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task SearchSpecies_WithValidRequest_Should_ReturnOk()
    {
        // arrange
        var command = new SearchSpeciesCommand();

        // act
        var (message, responses) = await httpClient
            .GETAsync<SearchSpeciesEndpoint, SearchSpeciesCommand, IEnumerable<SpeciesResponse>>(command);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        responses.Should().HaveCount(3).And.AllSatisfy(response =>
        {
            response.Id.Should().NotBeEmpty();
            response.Name.Should().NotBeNullOrWhiteSpace();
        });
    }
}