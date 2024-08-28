namespace Adoptrix.Logic.Options;

public class UserManagerOptions
{
    public const string SectionName = "UserManager";

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }

    /// <summary>
    /// The object ID of the API application service principal.
    /// </summary>
    public required string ApiObjectId { get; init; }
}
