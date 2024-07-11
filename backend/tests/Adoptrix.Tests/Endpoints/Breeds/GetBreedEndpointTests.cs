using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Breeds;

public class GetBreedEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.Client;

    [Theory, AdoptrixAutoData]
    public async Task GetBreed_WithKnownAnimalId_ShouldReturnOk(GetBreedRequest request, Breed breed)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var (message, response) = await httpClient.GETAsync<GetBreedEndpoint, GetBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(breed.Id);
    }

    [Theory, AdoptrixAutoData]
    public async Task GetBreed_WithUnknownAnimalId_ShouldReturnNotFound(GetBreedRequest request)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var testResult = await httpClient.GETAsync<GetBreedEndpoint, GetBreedRequest, BreedResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
