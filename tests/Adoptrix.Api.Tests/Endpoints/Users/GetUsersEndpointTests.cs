using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Users;

public class GetUsersEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task GetUsers_ShouldReturnOk()
    {
        // arrange
        var users = new List<UserResponse>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        fixture.UsersServiceMock.Setup(service => service.GetAllUsersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // act
        var (message, response) = await httpClient.GETAsync<GetUsersEndpoint, IEnumerable<UserResponse>>();

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().HaveCount(3);
    }
}
