using Adoptrix.Client.Services;

namespace Adoptrix.Client.Tests.Services;

public class GravatarUrlGeneratorTests
{
    [Fact]
    public void GetGravatarUrl_WithValidEmailAddress_ShouldReturnsExpectedUrl()
    {
        // arrange
        var generator = new GravatarUrlGenerator();
        const string emailAddress = "test@example.com";
        const int size = 64;
        const string defaultImage = "identicon";
        const string expectedHash = "973dfe463ec85785f5f95af5ba3906eedb2d931c24e69824a89ea65dba4e813b";

        // act
        var url = generator.GetGravatarUrl(emailAddress, size, defaultImage);

        // assert
        url.Should().Be($"https://gravatar.com/avatar/{expectedHash}?s={size}&d={defaultImage}");
    }
}
