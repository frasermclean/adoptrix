using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Species;

public class GetSpeciesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task GetSpecies_WithKnownSpeciesName_ShouldReturnOk(GetSpeciesRequest request, Core.Species species)
    {
        // arrange
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) = await httpClient.GETAsync<GetSpeciesEndpoint, GetSpeciesRequest, SpeciesResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Name.Should().Be(species.Name);
    }
}
