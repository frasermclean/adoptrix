using Adoptrix.Api.Services;
using Adoptrix.Contracts.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users/{userId:guid}")]
public class GetUserEndpoint(IUsersService usersService) : Endpoint<GetUserRequest, Results<Ok<UserResponse>, NotFound>>
{
    public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(GetUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await usersService.GetUserAsync(request.UserId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.NotFound();
    }
}
