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
        response.Version.Should().NotBeEmpty();
        response.Environment.Should().NotBeEmpty();
        response.BuildDate.Should().BeWithin(TimeSpan.FromMinutes(5));
        response.BuildDate.Kind.Should().Be(DateTimeKind.Utc);
    }
}
