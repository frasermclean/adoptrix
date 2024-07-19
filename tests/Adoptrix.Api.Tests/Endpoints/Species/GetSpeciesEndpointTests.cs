using System.Net;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Species;

public class GetSpeciesEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task GetSpecies_WithKnownSpeciesId_ShouldReturnOk(GetSpeciesRequest request, Core.Species species)
    {
        // arrange
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) = await httpClient.GETAsync<GetSpeciesEndpoint, GetSpeciesRequest, SpeciesResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(species.Id);
    }
}
