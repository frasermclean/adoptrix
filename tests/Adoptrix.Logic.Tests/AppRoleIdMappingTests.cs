using Adoptrix.Core;

namespace Adoptrix.Logic.Tests;

public class AppRoleIdMappingTests
{
    [Fact]
    public void GetAppRoleId_WithAdministratorUserRole_ShouldReturnAdministratorRoleId()
    {
        // act
        var appRoleId = AppRoleIdMapping.GetAppRoleId(UserRole.Administrator);

        // assert
        appRoleId.Should().Be(AppRoleIdMapping.AdministratorRoleId);
    }
}
