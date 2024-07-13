﻿using System.Net;
using Adoptrix.Core.Contracts.Requests.Breeds;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Breeds;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Tests.Endpoints.Breeds;

public class SearchBreedsEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.Client;

    [Theory, AdoptrixAutoData]
    public async Task SearchBreeds_WithValidRequest_ShouldReturnOk(SearchBreedsRequest request,
        List<BreedMatch> matchesToReturn)
    {
        // arrange
        app.BreedsRepositoryMock
            .Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchBreedsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(matchesToReturn);

        // act
        var testResult =
            await httpClient.GETAsync<SearchBreedsEndpoint, SearchBreedsRequest, List<BreedMatch>>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        testResult.Result.Should().BeEquivalentTo(matchesToReturn);
    }
}
