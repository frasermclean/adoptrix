using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Controllers;

public class UsersControllerTests(ApiFixture fixture) : ControllerTests(fixture), IClassFixture<ApiFixture>
{
    [Fact]
    public async Task GetCurrentUser_WithValidRequest_ShouldReturnOk()
    {
        // act
        var message = await HttpClient.GetAsync("api/users/me");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<UserResponse>();
        response.Should().NotBeNull();
        response!.Id.Should().Be(TestAuthHandler.TestUserId);
    }
}
