using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Application.Services.Abstractions;

namespace Adoptrix.Api.Tests.Controllers;

public abstract class ControllerTests(ApiFixture fixture)
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    protected HttpClient HttpClient => fixture.CreateClient();

    protected Mock<IAnimalsRepository> AnimalsRepositoryMock => fixture.AnimalsRepositoryMock;
    protected Mock<IBreedsRepository> BreedsRepositoryMock => fixture.BreedsRepositoryMock;
    protected Mock<ISpeciesRepository> SpeciesRepositoryMock => fixture.SpeciesRepositoryMock;

    protected static async Task<T> DeserializeJsonBody<T>(HttpResponseMessage message)
    {
        var obj = await message.Content.ReadFromJsonAsync<T>(SerializerOptions);
        return obj ?? throw new InvalidOperationException("Failed to deserialize JSON body.");
    }
}
