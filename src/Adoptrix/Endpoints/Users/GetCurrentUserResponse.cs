namespace Adoptrix.Endpoints.Users;

public class GetCurrentUserResponse
{
    public required string Name { get; init; }
    public required Guid UserId { get; init; }
}
