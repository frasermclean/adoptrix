using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;

namespace Adoptrix.Api.Tests.Endpoints.Users;

public class RemoveUserRoleEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    [Fact]
    public async Task RemoveUserRole_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new RemoveUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.RemoveUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse { Id = userId, Roles = [] });

        // act
        var (message, response) =
            await httpClient.DELETEAsync<RemoveUserRoleEndpoint, RemoveUserRoleRequest, UserResponse>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Id.Should().Be(userId);
        response.Roles.Should().BeEmpty();
    }

    [Fact]
    public async Task RemoveUserRole_WithNonExistingUser_ShouldReturnNotFound()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new RemoveUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.RemoveUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserNotFoundError(userId));

        // act
        var message = await httpClient.DELETEAsync<RemoveUserRoleEndpoint, RemoveUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveUserRole_WhenRoleIsNotAlreadyAssigned_ShouldReturnBadRequest()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.Administrator);
        var request = new RemoveUserRoleRequest { UserId = userId, Role = role };
        fixture.UserManagerMock.Setup(manager =>
                manager.RemoveUserRoleAssignmentAsync(userId, role, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserRoleNotAssignedError(role));

        // act
        var (message, response) =
            await httpClient.DELETEAsync<RemoveUserRoleEndpoint, RemoveUserRoleRequest, ErrorResponse>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainKey("role").WhoseValue.Should().Contain("Role is not assigned to the user.");
    }

    [Fact]
    public async Task RemoveUserRole_WithUnauthorizedUser_ShouldReturnForbidden()
    {
        // arrange
        var userId = Guid.NewGuid();
        const UserRole role = UserRole.Administrator;
        var httpClient = fixture.CreateClient(UserRole.User);
        var request = new RemoveUserRoleRequest { UserId = userId, Role = role };

        // act
        var message = await httpClient.DELETEAsync<RemoveUserRoleEndpoint, RemoveUserRoleRequest>(request);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
