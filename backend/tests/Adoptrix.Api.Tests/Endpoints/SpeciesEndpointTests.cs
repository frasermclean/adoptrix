using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Api.Tests.Fixtures.Mocks;

namespace Adoptrix.Api.Tests.Endpoints;

public class SpeciesEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk()
    {
        // act
        var message = await httpClient.GetAsync("api/species");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesResponse>>();
        fixture.SpeciesRepositoryMock.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        responses.Should().HaveCount(3).And.AllSatisfy(response =>
        {
            response.Id.Should().NotBeEmpty();
            response.Name.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task GetSpecies_WithValidId_ShouldReturnOk()
    {
        // arrange
        var speciesId = Guid.NewGuid();

        // act
        var message = await httpClient.GetAsync($"api/species/{speciesId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<SpeciesResponse>();
        response.Should().NotBeNull();
        fixture.SpeciesRepositoryMock.Verify(repository => repository.GetByIdAsync(speciesId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetSpecies_WithInvalidId_ShouldReturnNotFound()
    {
        // arrange
        var speciesId = SpeciesRepositoryMockSetup.UnknownSpeciesId;

        // act
        var message = await httpClient.GetAsync($"api/species/{speciesId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        fixture.SpeciesRepositoryMock.Verify(repository => repository.GetByIdAsync(speciesId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
