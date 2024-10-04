using Adoptrix.Core;

namespace Adoptrix.Logic;

public static class AppRoleIdMapping
{
    // set to internal so that it can be accessed by the tests
    internal static readonly Guid AdministratorRoleId = Guid.Parse("de833830-21b2-4373-9b22-b73b862d2d1f");

    private static readonly Dictionary<UserRole, Guid> RoleIds = new()
    {
        { UserRole.User, Guid.Parse("3afd2640-c285-47a7-a13e-58fb66cab1e0") },
        { UserRole.Administrator, AdministratorRoleId }
    };

    public static Guid GetAppRoleId(UserRole role) => RoleIds.TryGetValue(role, out var appRoleId)
        ? appRoleId
        : Guid.Empty;
}
