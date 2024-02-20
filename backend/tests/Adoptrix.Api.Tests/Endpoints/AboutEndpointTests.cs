using Adoptrix.Api.Endpoints;

namespace Adoptrix.Api.Tests.Endpoints;

public class AboutEndpointTests
{
    [Fact]
    public void AboutEndpoint_Should_ReturnVersionAndEnvironment()
    {
        // act
        var response = AboutEndpoint.Execute();

        // assert
        response.Version.Should().NotBeNullOrEmpty();
        response.Environment.Should().NotBeNullOrEmpty();
    }
}
