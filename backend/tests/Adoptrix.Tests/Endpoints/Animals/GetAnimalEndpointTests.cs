using System.Net;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Endpoints.Animals;
using Adoptrix.Tests.Mocks;

namespace Adoptrix.Tests.Endpoints.Animals;

public class GetAnimalEndpointTests(App app) : TestBase<App>
{
    [Fact]
    public async Task GetAnimal_WithKnownAnimalId_ShouldReturnOk()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var request = new GetAnimalRequest(animalId);

        // act
        var (message, response) =
            await app.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Id.Should().Be(animalId);
    }

    [Fact]
    public async Task GetAnimal_WithUnknownAnimalId_ShouldReturnNotFound()
    {
        // arrange
        var animalId = AnimalsRepositoryMockSetup.UnknownAnimalId;
        var request = new GetAnimalRequest(animalId);

        // act
        var testResult = await app.Client.GETAsync<GetAnimalEndpoint, GetAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
