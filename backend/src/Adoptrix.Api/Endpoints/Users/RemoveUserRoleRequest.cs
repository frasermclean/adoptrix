using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Users;

public class RemoveUserRoleRequest
{
    public Guid UserId { get; init; }
    public UserRole Role { get; init; }
}
