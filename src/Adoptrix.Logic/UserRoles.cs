namespace Adoptrix.Logic;

public static class UserRoles
{
    public const string Administrator = "Administrator";
    public const string User = "User";

    private static readonly Guid AdministratorRoleId = Guid.Parse("de833830-21b2-4373-9b22-b73b862d2d1f");

    public static Guid GetRoleId(string roleName) => roleName switch
    {
        Administrator => AdministratorRoleId,
        _ => Guid.Empty
    };
}
