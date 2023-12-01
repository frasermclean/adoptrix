using Adoptrix.Api.Extensions;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Users.GetCurrentUser;

public class GetCurrentUserEndpoint : EndpointWithoutRequest<GetCurrentUserResponse>
{
    public override void Configure()
    {
        Get("admin/users/me");
    }

    public override Task<GetCurrentUserResponse> ExecuteAsync(CancellationToken ct)
    {
        var response = new GetCurrentUserResponse
        {
            UserId = User.GetUserId()
        };

        return Task.FromResult(response);
    }
}