namespace Adoptrix.Core.Requests;

public class AddUserRoleRequest
{
    public Guid UserId { get; set; }
    public UserRole Role { get; set; }
}
