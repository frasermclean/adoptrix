using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class AddBreedEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

    [Theory, AdoptrixAutoData]
    public async Task AddBreed_WithValidRequest_ShouldReturnCreated(Core.Species species)
    {
        // arrange
        var request = CreateRequest(species.Id);
        fixture.SpeciesRepositoryMock
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

    [Fact]
    public async Task AddBreed_WithInvalidSpeciesId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest();
        fixture.SpeciesRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.SpeciesId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Core.Species);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddBreedEndpoint, AddBreedRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("speciesId");
    }

    private static AddBreedRequest CreateRequest(Guid? speciesId = null) => new()
    {
        Name = "Golden Retriever",
        SpeciesId = speciesId ?? Guid.NewGuid()
    };
}
