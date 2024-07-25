using System.Text.Json.Serialization;

namespace Adoptrix.Client.Models;

public class User
{
    [JsonPropertyName("id")] public Guid Id { get; init; }
    [JsonPropertyName("givenName")] public required string FirstName { get; set; }
    [JsonPropertyName("surname")] public required string LastName { get; set; }
    [JsonPropertyName("displayName")] public required string DisplayName { get; set; }
    [JsonPropertyName("mail")] public required string EmailAddress { get; init; }
}
