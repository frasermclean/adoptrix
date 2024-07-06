using System.Net;
using Adoptrix.Endpoints.Users;

namespace Adoptrix.Tests.Endpoints.Users;

public class GetCurrentUserEndpointTests(App app) : TestBase<App>
{
    private readonly HttpClient httpClient = app.BasicAuthClient;

    [Fact]
    public async Task GetCurrentUser_Should_ReturnExpectedResponse()
    {
        // act
        var (message, response) = await httpClient.GETAsync<GetCurrentUserEndpoint, GetCurrentUserResponse>();

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Name.Should().Be(BasicAuthHandler.UserName);
        response.UserId.Should().Be(BasicAuthHandler.UserId);
    }
}
