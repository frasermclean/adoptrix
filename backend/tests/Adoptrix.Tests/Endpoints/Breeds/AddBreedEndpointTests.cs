using System.Net;
using Adoptrix.Core;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Breeds;

public class AddBreedEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task AddBreed_WithValidRequest_ShouldReturnCreated(AddBreedRequest request, Species species)
    {
        // arrange
        app.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(species);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, BreedResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be(request.Name);
    }

    [Theory, AdoptrixAutoData]
    public async Task AddBreed_WithInvalidSpeciesId_ShouldReturnBadRequest(AddBreedRequest request)
    {
        // arrange
        app.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Species);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesId");
    }
}
