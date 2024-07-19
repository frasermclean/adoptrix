using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adoptrix.Web.Extensions;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static async Task<T?> DeserializeJsonContentAsync<T>(this HttpResponseMessage message, CancellationToken cancellationToken = default)
    {
        var json = await message.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, SerializerOptions);
    }
}
