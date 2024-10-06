using Adoptrix.Api.Security;
using Adoptrix.Api.Services;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Endpoints.Users;

public class GetUsersEndpoint(IUserManager userManager) : EndpointWithoutRequest<IEnumerable<UserResponse>>
{
    public override void Configure()
    {
        Get("users");
        Permissions(PermissionNames.UsersManage);
    }

    public override async Task<IEnumerable<UserResponse>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await userManager.GetAllUsersAsync(cancellationToken);
    }
}
