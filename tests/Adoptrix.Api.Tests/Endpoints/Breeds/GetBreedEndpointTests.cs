﻿using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Breeds;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetBreedEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task GetBreed_WithKnownId_ShouldReturnOk()
    {
        // arrange
        const int breedId = 1;

        // act
        var message = await httpClient.GetAsync($"api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<BreedResponse>();
        response!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetBreed_WithUnknownId_ShouldReturnNotFound()
    {
        // arrange
        const int breedId = -1;

        // act
        var message = await httpClient.GetAsync($"api/breeds/{breedId}");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
