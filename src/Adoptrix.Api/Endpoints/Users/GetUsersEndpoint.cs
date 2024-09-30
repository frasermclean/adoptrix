using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;

namespace Adoptrix.Api.Endpoints.Users;

[HttpGet("users")]
public class GetUsersEndpoint(IUserManager userManager) : EndpointWithoutRequest<IEnumerable<UserResponse>>
{
    public override async Task<IEnumerable<UserResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var users = await userManager.GetAllUsersAsync(cancellationToken);

        return users;
    }
}
