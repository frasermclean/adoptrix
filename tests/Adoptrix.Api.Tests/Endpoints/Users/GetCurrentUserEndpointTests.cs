using System.Net;
using Adoptrix.Api.Endpoints.Users;

namespace Adoptrix.Api.Tests.Endpoints.Users;

public class GetCurrentUserEndpointTests(ApiFixture fixture) : TestBase<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.BasicAuthClient;

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
