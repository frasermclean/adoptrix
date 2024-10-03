using Adoptrix.Api.Extensions;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Services;

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

        return result.IsSuccess
            ? result.Value
            : throw new InvalidOperationException("User not found");
    }
}
