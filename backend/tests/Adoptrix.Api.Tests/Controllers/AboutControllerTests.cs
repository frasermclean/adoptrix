using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Controllers;

public class AboutControllerTests(ApiFixture fixture) : ControllerTests(fixture), IClassFixture<ApiFixture>
{
    [Fact]
    public async Task GetAbout_WithValidRequest_ShouldReturnOk()
    {
        // act
        var message = await HttpClient.GetAsync("api/about");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<AboutResponse>();
        response.Should().NotBeNull();
        response!.Version.Should().NotBeEmpty();
        response.Environment.Should().NotBeEmpty();
        response.BuildDate.Should().BeWithin(TimeSpan.FromMinutes(5));
        response.BuildDate.Kind.Should().Be(DateTimeKind.Utc);
    }
}
