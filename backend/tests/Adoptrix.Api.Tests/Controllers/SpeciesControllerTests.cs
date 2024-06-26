﻿using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Api.Tests.Fixtures.Mocks;
using Adoptrix.Application.Features.Species.Queries;
using Adoptrix.Application.Features.Species.Responses;

namespace Adoptrix.Api.Tests.Controllers;

public class SpeciesControllerTests(ApiFixture fixture) : ControllerTests(fixture), IClassFixture<ApiFixture>
{
    [Fact]
    public async Task SearchSpecies_WithValidRequest_ShouldReturnOk()
    {
        // act
        var message = await HttpClient.GetAsync("api/species");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var matches = await message.Content.ReadFromJsonAsync<IEnumerable<SearchSpeciesMatch>>();
        SpeciesRepositoryMock.Verify(repository => repository.SearchAsync(It.IsAny<SearchSpeciesQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
        matches.Should().HaveCount(3).And.AllSatisfy(match =>
        {
            match.SpeciesId.Should().NotBeEmpty();
            match.SpeciesName.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task GetSpecies_WithValidId_ShouldReturnOk()
    {
        // arrange
        var speciesId = Guid.NewGuid();

        // act
        var message = await HttpClient.GetAsync($"api/species/{speciesId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<SpeciesResponse>();
        response.Should().NotBeNull();
        SpeciesRepositoryMock.Verify(repository => repository.GetByIdAsync(speciesId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetSpecies_WithInvalidId_ShouldReturnNotFound()
    {
        // arrange
        var speciesId = SpeciesRepositoryMockSetup.UnknownSpeciesId;

        // act
        var message = await HttpClient.GetAsync($"api/species/{speciesId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        SpeciesRepositoryMock.Verify(repository => repository.GetByIdAsync(speciesId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
