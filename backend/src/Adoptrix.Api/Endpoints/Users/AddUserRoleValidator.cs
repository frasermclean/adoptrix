using FluentValidation;

namespace Adoptrix.Api.Endpoints.Users;

public class AddUserRoleValidator : Validator<AddUserRoleRequest>
{
    public AddUserRoleValidator()
    {
        RuleFor(r => r.UserId).NotEmpty();
        RuleFor(r => r.Role).IsInEnum();
    }
}
