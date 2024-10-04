using Adoptrix.Api.Security;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Users;

public class RemoveUserRoleEndpoint(IUserManager userManager)
    : Endpoint<RemoveUserRoleRequest, Results<Ok<UserResponse>, NotFound, ErrorResponse>>
{
    public override void Configure()
    {
        Delete("users/{userId:guid}/roles/{role}");
        Permissions(PermissionNames.UsersManage);
    }

    public override async Task<Results<Ok<UserResponse>, NotFound, ErrorResponse>> ExecuteAsync(RemoveUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userManager.RemoveUserRoleAssignmentAsync(request.UserId, request.Role, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.Value);
        }

        if (result.HasError<UserNotFoundError>())
        {
            return TypedResults.NotFound();
        }

        if (result.HasError<UserRoleNotAssignedError>())
        {
            AddError(r => r.Role, "Role is not assigned to the user.");
        }

        return new ErrorResponse(ValidationFailures);
    }
}
