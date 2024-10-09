using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;

namespace Adoptrix.Api.Tests.Endpoints.Users;

public class GetCurrentUserEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient(UserRole.User);

    [Fact]
    public async Task GetCurrentUser_WithValidRequest_ShouldReturnOk()
    {
        // arrange
        fixture.UserManagerMock.Setup(manager => manager.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserResponse());

        // act
        var message = await httpClient.GetAsync("api/users/me");

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
