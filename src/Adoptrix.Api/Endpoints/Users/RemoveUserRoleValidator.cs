using FluentValidation;

namespace Adoptrix.Api.Endpoints.Users;

public class RemoveUserRoleValidator : Validator<RemoveUserRoleRequest>
{
    public RemoveUserRoleValidator()
    {
        RuleFor(request => request.UserId).NotEmpty();
        RuleFor(request => request.Role).IsInEnum();
    }
}
