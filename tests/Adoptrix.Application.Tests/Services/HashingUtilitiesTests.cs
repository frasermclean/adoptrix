using Adoptrix.Application.Services;

namespace Adoptrix.Application.Tests.Services;

public class HashingUtilitiesTests
{
    private readonly IHashGenerator hashGenerator = new HashGenerator();

    [Theory]
    [InlineData("foo", "bar")]
    [InlineData("quas", "wex", "exort")]
    [InlineData("supercalafragalisticexpialadocious")]
    public void ComputeHash_GivenValues_ReturnsHash(params string[] values)
    {
        // act
        var hash = hashGenerator.ComputeHash(values);

        // assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().HaveLength(32);
        hash.Should().MatchRegex("^[a-z0-9]*$");
    }
}