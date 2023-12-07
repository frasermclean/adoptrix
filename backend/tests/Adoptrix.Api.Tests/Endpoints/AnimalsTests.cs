using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Animals.GetAnimal;
using Adoptrix.Api.Endpoints.Animals.SearchAnimals;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalsTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    : TestClass<ApiTestFixture>(fixture, outputHelper)
{
    private readonly HttpClient httpClient = fixture.Client;
    private readonly ISqidConverter sqidConverter = fixture.Services.GetRequiredService<ISqidConverter>();

    [Fact]
    public async Task SearchAnimals_WithValidRequest_Should_ReturnOk()
    {
        var command = new SearchAnimalsCommand();

        // act
        var (message, responses) = await httpClient.GETAsync<SearchAnimalsEndpoint, SearchAnimalsCommand, IEnumerable<AnimalResponse>>(command);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        responses.Should().HaveCount(3).And.AllSatisfy(ValidateAnimalResponse);
    }

    [Fact]
    public async Task GetAnimal_WithValidRequest_Should_ReturnOk()
    {
        // arrange
        var id = sqidConverter.ConvertToSqid(3);
        var command = new GetAnimalCommand { Id = id };

        // act
        var (message, response) = await httpClient.GETAsync<GetAnimalEndpoint, GetAnimalCommand, AnimalResponse>(command);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        ValidateAnimalResponse(response);

    }

    private static void ValidateAnimalResponse(AnimalResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.Species.Should().NotBeEmpty();
        response.Breed.Should().NotBeEmpty();
        response.Sex.Should().NotBeNull();
        response.DateOfBirth.Should().NotBe(default);
    }
}