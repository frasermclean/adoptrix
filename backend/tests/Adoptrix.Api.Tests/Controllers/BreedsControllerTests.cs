﻿using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Api.Tests.Fixtures.Mocks;
using Adoptrix.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Tests.Controllers;

public class BreedsControllerTests(ApiFixture fixture) : ControllerTests(fixture), IClassFixture<ApiFixture>
{
    [Theory, AutoData]
    public async Task GetBreed_WithValidBreedId_Returns_Ok(Guid breedId)
    {
        // act
        var message = await HttpClient.GetAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await DeserializeJsonBody<BreedResponse>(message);
        ValidateBreedResponse(response);
    }

    [Fact]
    public async Task GetBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;

        // act
        var message = await HttpClient.GetAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        var details = await DeserializeJsonBody<ProblemDetails>(message);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Fact]
    public async Task SearchBreeds_WithValidRequest_Returns_Ok()
    {
        // act
        var message = await HttpClient.GetAsync("api/breeds");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var responses = await DeserializeJsonBody<IEnumerable<BreedResponse>>(message);
        responses.Should().HaveCount(ApiFixture.SearchResultsCount).And.AllSatisfy(ValidateBreedResponse);
    }

    [Fact]
    public async Task AddBreed_WithValidRequest_Returns_Created()
    {
        // arrange
        const string breedName = "Sausage Dog";
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => null);
        var request = new SetBreedRequest(breedName, Guid.NewGuid());

        // act
        var message = await HttpClient.PostAsync("api/breeds", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        var response = await DeserializeJsonBody<BreedResponse>(message);
        ValidateBreedResponse(response);
    }

    [Fact]
    public async Task AddBreed_WithDuplicateBreedName_Returns_ProblemDetails()
    {
        // arrange
        var request = CreateRequest();

        // act
        var message = await HttpClient.PostAsync("api/breeds", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await DeserializeJsonBody<ValidationProblemDetails>(message);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task UpdateBreed_WithValidRequest_Should_Return_Ok()
    {
        // arrange
        var breedId = Guid.NewGuid();
        const string breedName = "Golden Retriever";
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(breedName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => null);
        var request = CreateRequest(breedName);

        // act
        var message = await HttpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await DeserializeJsonBody<BreedResponse>(message);
        ValidateBreedResponse(response);
    }

    [Fact]
    public async Task UpdateBreed_WithDuplicateBreedName_Returns_ProblemDetails()
    {
        // arrange
        var breedId = Guid.NewGuid();
        var request = CreateRequest();

        // act
        var message = await HttpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await DeserializeJsonBody<ValidationProblemDetails>(message);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().ContainKey("Name");
    }

    [Fact]
    public async Task UpdateBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;
        var request = CreateRequest("Schnauzer");
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await HttpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
        var details = await DeserializeJsonBody<ProblemDetails>(message);
        details.Should().BeOfType<ProblemDetails>().Which.Title.Should().Be("Not Found");
    }

    [Theory, AutoData]
    public async Task DeleteBreed_WithValidBreedId_Returns_NoContent(Guid breedId)
    {
        // act
        var message = await HttpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteBreed_WithUnknownBreedId_Returns_NotFound()
    {
        // arrange
        var breedId = BreedsRepositoryMockSetup.UnknownBreedId;

        // act
        var message = await HttpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static SetBreedRequest CreateRequest(string name = "Corgi", Guid? speciesId = null)
        => new(name, speciesId ?? Guid.NewGuid());

    private static void ValidateBreedResponse(BreedResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.SpeciesId.Should().NotBeEmpty();
    }
}