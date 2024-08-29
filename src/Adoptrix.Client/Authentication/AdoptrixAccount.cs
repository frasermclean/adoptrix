using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Adoptrix.Client.Authentication;

public class AdoptrixAccount : RemoteUserAccount
{
    [JsonPropertyName("oid")] public string? Oid { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}
