using System.Net;
using Adoptrix.Api.Endpoints.Breeds;
using Adoptrix.Contracts.Requests;
using Adoptrix.Persistence.Responses;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

public class SearchBreedsEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk(SearchBreedsRequest request,
        List<SearchBreedsItem> items)
    {
        // arrange
        fixture.BreedsRepositoryMock
            .Setup(repository =>
                repository.SearchAsync(It.IsAny<Guid?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        // act
        var testResult =
            await httpClient.GETAsync<SearchBreedsEndpoint, SearchBreedsRequest, List<SearchBreedsItem>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(items);
    }
}
