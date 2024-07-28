namespace Adoptrix.Contracts.Responses;

public class UserResponse
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? DisplayName { get; init; }
    public string? EmailAddress { get; init; }
    public string? Role { get; init; }
    public bool IsCurrentUser { get; init; }
}
