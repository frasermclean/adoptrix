using Adoptrix.Client.Extensions;

namespace Adoptrix.Client.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("SSBhbSBhIHNpbGx5IHNhaWxvcg==")]
    [InlineData("VGhlIGNhdCBzYXQgb24gdGhlIG1hdA==")]
    public void DecodeBase64Url_Should_ReturnExpectedValue(string base64String)
    {
        // arrange
        var base64UrlString = base64String
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        // act
        var output = base64UrlString.DecodeBase64Url();

        // assert
        output.Should().Be(base64String);
    }
}
