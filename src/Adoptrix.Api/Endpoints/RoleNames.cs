using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints;

public static class RoleNames
{
    public const string User = nameof(UserRole.User);
    public const string Administrator = nameof(UserRole.Administrator);

    public static string FromRole(UserRole role) => role switch
    {
        UserRole.Administrator => Administrator,
        _ => User
    };
}
