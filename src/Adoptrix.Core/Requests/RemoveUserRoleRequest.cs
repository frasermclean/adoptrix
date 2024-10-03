namespace Adoptrix.Core.Requests;

public class RemoveUserRoleRequest
{
    public Guid UserId { get; init; }
    public UserRole Role { get; init; }
}
