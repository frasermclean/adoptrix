using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;

namespace Adoptrix.Api.Tests.Endpoints.Users;

public class AddUserRoleEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    [Fact]
    public async Task AddUserRole_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new AddUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.AddUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse { Id = userId, Roles = [role] });

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddUserRoleEndpoint, AddUserRoleRequest, UserResponse>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Id.Should().Be(userId);
        response.Roles.Should().ContainSingle(r => r == role);
    }

    [Fact]
    public async Task AddUserRole_WithNonExistingUser_ShouldReturnNotFound()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new AddUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.AddUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserNotFoundError(userId));

        // act
        var message = await httpClient.POSTAsync<AddUserRoleEndpoint, AddUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddUserRole_WithAlreadyAssignedRole_ShouldReturnBadRequest()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new AddUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.AddUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRoleAlreadyAssignedError(role));

        // act
        var message = await httpClient.POSTAsync<AddUserRoleEndpoint, AddUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddUserRole_WithInvalidRole_ShouldReturnBadRequest()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = (UserRole) 42;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new AddUserRoleRequest { UserId = userId, Role = role };

        // act
        var message = await httpClient.POSTAsync<AddUserRoleEndpoint, AddUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddUserRole_WithUnauthorizedUser_ShouldReturnForbidden()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.User);
        var request = new AddUserRoleRequest { UserId = userId, Role = role };

        // act
        var message = await httpClient.POSTAsync<AddUserRoleEndpoint, AddUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
