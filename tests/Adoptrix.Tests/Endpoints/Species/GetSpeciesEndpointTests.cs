using System.Net;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Species;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Species;

public class GetSpeciesEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.Client;

    [Theory, AdoptrixAutoData]
    public async Task GetSpecies_WithKnownSpeciesId_ShouldReturnOk(GetSpeciesRequest request, Core.Species species)
    {
        // arrange
        app.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) = await httpClient.GETAsync<GetSpeciesEndpoint, GetSpeciesRequest, SpeciesResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(species.Id);
    }
}
