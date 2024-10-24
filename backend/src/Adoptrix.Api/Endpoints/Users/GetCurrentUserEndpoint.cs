using Adoptrix.Api.Services;
using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Users;

public class GetCurrentUserEndpoint(IUserManager userManager, IRequestContext requestContext)
    : EndpointWithoutRequest<UserResponse>
{
    public override void Configure()
    {
        Get("users/me");
    }

    public override async Task<UserResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = await userManager.GetUserAsync(requestContext.UserId, cancellationToken);

        return result.Value;
    }
}
