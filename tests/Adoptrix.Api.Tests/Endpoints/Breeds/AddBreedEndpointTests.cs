using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class AddBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

    [Theory, AdoptrixAutoData]
    public async Task AddBreed_WithValidRequest_ShouldReturnCreated(Core.Species species)
    {
        // arrange
        var request = CreateRequest(species.Name);
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetAsync(species.Name, It.IsAny<CancellationToken>()))
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

    [Fact]
    public async Task AddBreed_WithInvalidSpeciesName_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest();
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetAsync(request.SpeciesName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Core.Species);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesName");
    }

    private static AddBreedRequest CreateRequest(string? speciesName = null) => new()
    {
        Name = "Golden Retriever",
        SpeciesName = speciesName ?? "Dog"
    };
}
