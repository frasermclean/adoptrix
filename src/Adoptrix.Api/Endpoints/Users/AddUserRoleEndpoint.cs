using Adoptrix.Api.Security;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Users;

public class AddUserRoleEndpoint(IUserManager userManager)
    : Endpoint<AddUserRoleRequest, Results<Ok<UserResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Post("users/{userId:guid}/role");
        Permissions(PermissionNames.UsersManage);
    }

    public override async Task<Results<Ok<UserResponse>, NotFound, ErrorResponse>> ExecuteAsync(AddUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userManager.AddUserRoleAssignmentAsync(request.UserId, request.Role, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        if (result.HasError<UserNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        if (result.HasError<UserRoleAlreadyAssignedError>())
        {
            AddError(r => r.Role, "User role is already assigned to this user");
        }

        return new ErrorResponse(ValidationFailures);
    }
}
