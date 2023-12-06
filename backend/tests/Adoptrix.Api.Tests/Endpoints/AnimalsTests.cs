using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Animals.GetAnimal;
using Adoptrix.Application.Commands.Animals;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalsTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    : TestClass<ApiTestFixture>(fixture, outputHelper)
{
    private readonly HttpClient httpClient = fixture.Client;

    [Fact]
    public async Task GetAnimal_WithValidRequest_Should_ReturnOk()
    {
        // arrange
        var command = new GetAnimalCommand { Id = "UmSU" };

        // act
        var (message, response) = await httpClient.GETAsync<GetAnimalEndpoint, GetAnimalCommand, AnimalResponse>(command);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}