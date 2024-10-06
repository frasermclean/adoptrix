using Adoptrix.Api.Security;
using Adoptrix.Api.Services;
using Adoptrix.Core.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Users;

public class GetUserEndpoint(IUserManager userManager) : EndpointWithoutRequest<Results<Ok<UserResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("users/{userId:guid}");
        Permissions(PermissionNames.UsersManage);
    }

    public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var userId = Route<Guid>("userId");
        var result = await userManager.GetUserAsync(userId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
