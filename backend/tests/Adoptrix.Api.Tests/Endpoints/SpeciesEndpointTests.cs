using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints;

public class SpeciesEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task SearchSpecies_WithValidRequest_Should_ReturnOk()
    {
        // act
        var message = await httpClient.GetAsync("api/species");
        var responses = await message.Content.ReadFromJsonAsync<IEnumerable<SpeciesResponse>>();

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        fixture.SpeciesRepository.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        responses.Should().HaveCount(3).And.AllSatisfy(response =>
        {
            response.Id.Should().NotBeEmpty();
            response.Name.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Theory]
    [InlineData("dog")]
    [InlineData("e936b936-650c-4c33-abdf-8d5287b809e8")]
    public async Task GetSpecies_WithValidIdOrName_Should_Return_Ok(string speciesIdOrName)
    {
        // act
        var message = await httpClient.GetAsync($"api/species/{speciesIdOrName}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetSpecies_WithInvalidId_Should_Return_NotFound()
    {
        // arrange
        var speciesId = Guid.Empty;

        // act
        var message = await httpClient.GetAsync($"api/species/{speciesId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        fixture.SpeciesRepository.Verify(repository => repository.GetByIdAsync(speciesId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
