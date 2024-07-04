using System.Net;
using Adoptrix.Endpoints.About;

namespace Adoptrix.Tests.Endpoints.About;

public class AboutEndpointTests(App app) : TestBase<App>
{
    [Fact]
    public async Task GetAbout_Should_ReturnAboutResponse()
    {
        // act
        var (message, response) = await app.Client.GETAsync<AboutEndpoint, AboutResponse>();

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Version.Should().NotBeEmpty();
        response.BuildDate.Should().BeBefore(DateTime.UtcNow);
        response.Environment.Should().NotBeEmpty();
    }
}
