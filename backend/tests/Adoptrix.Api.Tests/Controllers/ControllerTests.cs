using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Controllers;

public abstract class ControllerTests(ApiFixture fixture)
{
    protected readonly HttpClient HttpClient = fixture.CreateClient();

    protected static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
}
