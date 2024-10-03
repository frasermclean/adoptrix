namespace Adoptrix.Core.Responses;

public class UserResponse
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? DisplayName { get; init; }
    public string? EmailAddress { get; init; }
    public IEnumerable<UserRole> Roles { get; init; } = [];
}
