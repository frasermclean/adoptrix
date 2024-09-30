using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users/{userId:guid}")]
public class GetUserEndpoint(IUserManager userManager) : Endpoint<GetUserRequest, Results<Ok<UserResponse>, NotFound>>
{
    public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(GetUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await userManager.GetUserAsync(request.UserId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
