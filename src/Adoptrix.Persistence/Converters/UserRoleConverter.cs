using Adoptrix.Core;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adoptrix.Persistence.Converters;

public class UserRoleConverter()
    : ValueConverter<UserRole, char>(role => UserRoleToChar(role), c => CharToUserRole(c))
{
    private const char UserChar = 'U';
    private const char AdministratorChar = 'A';

    private static char UserRoleToChar(UserRole role) => role switch
    {
        UserRole.Administrator => AdministratorChar,
        _ => UserChar
    };

    private static UserRole CharToUserRole(char c) => c switch
    {
        AdministratorChar => UserRole.Administrator,
        _ => UserRole.User
    };
}
