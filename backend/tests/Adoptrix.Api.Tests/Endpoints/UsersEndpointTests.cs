using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints;

public class UsersEndpointTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private readonly HttpClient httpClient = fixture.CreateClient();

    [Fact]
    public async Task GetUserEndpoint_Should_Return_ExpectedResponse()
    {
        // act
        var message = await httpClient.GetAsync("api/users/me");
        var response = await message.Content.ReadFromJsonAsync<UserResponse>();

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Should().NotBeNull();
        response!.Id.Should().Be(TestAuthHandler.TestUserId);
    }
}
