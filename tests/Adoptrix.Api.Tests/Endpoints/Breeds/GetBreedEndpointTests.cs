using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class GetBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task GetBreed_WithKnownAnimalId_ShouldReturnOk(GetBreedRequest request, Breed breed)
    {
        // arrange
        fixture.BreedsRepositoryMock
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
        fixture.BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var testResult = await httpClient.GETAsync<GetBreedEndpoint, GetBreedRequest, BreedResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
