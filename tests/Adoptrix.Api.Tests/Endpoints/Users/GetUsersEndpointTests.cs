using System.Net;
using Adoptrix.Api.Endpoints.Users;
using Adoptrix.Contracts.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Users;

[Collection(nameof(ApiCollection))]
[Trait("Category", "Integration")]
public class GetUsersEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.AdminClient;

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
