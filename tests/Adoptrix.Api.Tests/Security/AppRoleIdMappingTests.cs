using Adoptrix.Api.Security;
using Adoptrix.Core;

namespace Adoptrix.Api.Tests.Security;

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
