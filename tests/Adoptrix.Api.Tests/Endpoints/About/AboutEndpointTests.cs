using System.Net;
using Adoptrix.Api.Endpoints.About;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints.About;

public class AboutEndpointTests(MockServicesFixture fixture) : TestBase<MockServicesFixture>
{
    [Fact]
    public async Task GetAbout_Should_ReturnAboutResponse()
    {
        // arrange
        var containerUri = new Uri("https://localhost/animal-images");
        fixture.AnimalImagesBlobContainerManagerMock.Setup(manager => manager.ContainerUri)
            .Returns(containerUri);

        // act
        var (message, response) = await fixture.Client.GETAsync<AboutEndpoint, AboutResponse>();

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Version.Should().NotBeEmpty();
        response.BuildDate.Should().BeBefore(DateTime.UtcNow);
        response.Environment.Should().NotBeEmpty();
    }
}
