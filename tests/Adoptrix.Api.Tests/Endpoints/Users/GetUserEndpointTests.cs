using System.Net;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;

namespace Adoptrix.Api.Tests.Endpoints.Users;

[Trait("Category", "Integration")]
public class GetUserEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient(UserRole.Administrator);

    [Fact]
    public async Task GetUser_WithValidId_ShouldReturnOk()
    {
        // arrange
        var userId = Guid.NewGuid();
        fixture.UserManagerMock.Setup(manager => manager.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse { Id = userId });

        // act
        var message = await httpClient.GetAsync($"api/users/{userId}");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUser_WithInvalidId_ShouldReturnNotFound()
    {
        // arrange
        var userId = Guid.NewGuid();
        fixture.UserManagerMock.Setup(manager => manager.GetUserAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserNotFoundError(userId));

        // act
        var message = await httpClient.GetAsync($"api/users/{userId}");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
