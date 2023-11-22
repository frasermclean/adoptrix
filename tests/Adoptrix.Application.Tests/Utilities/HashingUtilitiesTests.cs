using Adoptrix.Application.Utilities;

namespace Adoptrix.Application.Tests.Utilities;

public class HashingUtilitiesTests
{
    [Theory]
    [InlineData("foo", "bar")]
    [InlineData("quas", "wex", "exort")]
    [InlineData("supercalafragalisticexpialadocious")]
    public void ComputeHash_GivenValues_ReturnsHash(params string[] values)
    {

        // act
        var hash = HashingUtilities.ComputeHash(values);

        // assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().HaveLength(32);
        hash.Should().MatchRegex("^[a-z0-9]*$");
    }
}