using Adoptrix.Api.Extensions;
using Adoptrix.Api.Services;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Endpoints.Users;

public class GetCurrentUserEndpoint(IUserManager userManager) : EndpointWithoutRequest<UserResponse>
{
    public override void Configure()
    {
        Get("users/me");
    }

    public override async Task<UserResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var result = await userManager.GetUserAsync(userId, cancellationToken);

        return result.Value;
    }
}
