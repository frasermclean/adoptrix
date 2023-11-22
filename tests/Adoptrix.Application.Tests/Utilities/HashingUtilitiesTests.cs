using Adoptrix.Application.Utilities;

namespace Adoptrix.Application.Tests.Utilities;

public class HashingUtilitiesTests
{
    [Fact]
    public void ComputeHash_GivenValues_ReturnsHash()
    {
        // arrange
        var values = new[] { "foo", "bar" };

        // act
        var hash = HashingUtilities.ComputeHash(values);

        // assert
        hash.Should().NotBeNullOrEmpty();
    }
}