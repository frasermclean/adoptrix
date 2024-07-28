namespace Adoptrix.Contracts.Responses;

public class UserResponse
{
    public required string Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? DisplayName { get; init; }
    public string? EmailAddress { get; init; }
}
