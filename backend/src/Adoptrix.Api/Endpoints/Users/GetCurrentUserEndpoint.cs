using System.Security.Claims;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Extensions;

namespace Adoptrix.Api.Endpoints.Users;

public static class GetCurrentUserEndpoint
{
    public static UserResponse Execute(ClaimsPrincipal claimsPrincipal)
    {
        return new UserResponse
        {
            Id = claimsPrincipal.GetUserId()
        };
    }

}
