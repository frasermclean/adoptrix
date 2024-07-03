using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Requests;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Domain.Contracts.Responses;
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

    [Theory, AutoData]
    public async Task GetBreed_WithUnknownBreedId_Returns_NotFound(Guid breedId)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(breedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

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
        var responses = await DeserializeJsonBody<IEnumerable<BreedMatch>>(message);
        responses.Should().HaveCount(ApiFixture.SearchResultsCount);
    }

    [Theory, AutoData]
    public async Task AddBreed_WithValidRequest_Returns_Created(SetBreedRequest request)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await HttpClient.PostAsync("api/breeds", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        var response = await DeserializeJsonBody<BreedResponse>(message);
        ValidateBreedResponse(response);
    }

    [Theory, AutoData]
    public async Task AddBreed_WithDuplicateBreedName_ReturnsProblemDetails(SetBreedRequest request, Breed breed)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(breed);

        // act
        var message = await HttpClient.PostAsync("api/breeds", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await DeserializeJsonBody<ValidationProblemDetails>(message);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().NotBeEmpty();
    }

    [Theory, AutoData]
    public async Task UpdateBreed_WithValidRequest_Should_Return_Ok(Guid breedId, SetBreedRequest request)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, CancellationToken _) => null);

        // act
        var message = await HttpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await DeserializeJsonBody<BreedResponse>(message);
        ValidateBreedResponse(response);
    }

    [Theory, AutoData]
    public async Task UpdateBreed_WithDuplicateBreedName_Returns_ProblemDetails(Guid breedId, SetBreedRequest request)
    {
        // act
        var message = await HttpClient.PutAsync($"/api/breeds/{breedId}", JsonContent.Create(request));

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        var details = await DeserializeJsonBody<ValidationProblemDetails>(message);
        details.Should().BeOfType<ValidationProblemDetails>().Which.Errors.Should().ContainSingle()
            .Which.Key.Should().Be("name");
    }

    [Theory, AutoData]
    public async Task UpdateBreed_WithUnknownBreedId_ReturnsNotFound(Guid breedId, SetBreedRequest request)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByNameAsync(request.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);
        BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(breedId, It.IsAny<CancellationToken>()))
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

    [Theory, AutoData]
    public async Task DeleteBreed_WithUnknownBreedId_Returns_NotFound(Guid breedId)
    {
        // arrange
        BreedsRepositoryMock
            .Setup(repository => repository.GetByIdAsync(breedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Breed);

        // act
        var message = await HttpClient.DeleteAsync($"/api/breeds/{breedId}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    private static void ValidateBreedResponse(BreedResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeNullOrWhiteSpace();
        response.SpeciesId.Should().NotBeEmpty();
    }
}
