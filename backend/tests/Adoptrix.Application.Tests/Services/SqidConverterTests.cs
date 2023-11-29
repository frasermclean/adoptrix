﻿using Adoptrix.Application.Services;
using Microsoft.Extensions.Options;
using Sqids;

namespace Adoptrix.Application.Tests.Services;

public class SqidConverterTests
{
    private readonly ISqidConverter sqidConverter;

    public SqidConverterTests()
    {
        var options = new SqidsOptions
        {
            Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789",
            MinLength = 5,
        };

        sqidConverter = new SqidConverter(Options.Create(options));
    }

    [Theory]
    [InlineData(1, "52upd")]
    [InlineData(69, "1k1p4")]
    [InlineData(420, "nkoim")]
    public void ConvertToSqid_Should_Return_ExpectedResult(int valueToConvert, string expectedResult)
    {
        // act
        var result = sqidConverter.ConvertToSqid(valueToConvert);

        // assert
        result.Should().Be(expectedResult);
        result.Should().HaveLength(5);
        result.Should().MatchRegex("^[a-z0-9]*$");
    }

    [Theory]
    [InlineData("52upd", 1)]
    [InlineData("1k1p4", 69)]
    [InlineData("nkoim", 420)]
    public void ConvertToInt_Should_Return_ExpectedResult(string valueToConvert, int expectedResult)
    {
        // act
        var result = sqidConverter.ConvertToInt(valueToConvert);
        var isValid = sqidConverter.IsValid(valueToConvert);

        // assert
        result.Should().Be(expectedResult);
        isValid.Should().BeTrue();
    }

    [Fact]
    public void ConvertToInt_WithInvalidSqid_Should_ThrowException()
    {
        // arrange
        const string invalidSqid = "XXX";

        // act
        Action act = () => sqidConverter.ConvertToInt(invalidSqid);
        var isValid = sqidConverter.IsValid(invalidSqid);

        // assert
        act.Should().Throw<ArgumentException>().Which.Message.Should().Be("Invalid value to decode (Parameter 'sqid')");
        isValid.Should().BeFalse();
    }
}