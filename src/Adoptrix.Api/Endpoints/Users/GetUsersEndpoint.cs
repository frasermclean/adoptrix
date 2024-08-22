using Adoptrix.Contracts.Responses;
using Adoptrix.Logic.Services;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users")]
public class GetUsersEndpoint(IUsersService usersService) : EndpointWithoutRequest<IEnumerable<UserResponse>>
{
    public override async Task<IEnumerable<UserResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var users = await usersService.GetAllUsersAsync(cancellationToken);

        return users;
    }
}
