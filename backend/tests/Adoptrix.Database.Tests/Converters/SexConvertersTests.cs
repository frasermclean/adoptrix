using Adoptrix.Database.Converters;
using Adoptrix.Domain.Models;

namespace Adoptrix.Database.Tests.Converters;

public class SexConvertersTests
{
    private readonly SexConverter converter = new();

    [Theory]
    [InlineData(Sex.Male, 'M')]
    [InlineData(Sex.Female, 'F')]
    [InlineData(Sex.Unknown, 'X')]
    public void ConvertToProviderTyped_Should_Return_ExpectedResult(Sex valueToConvert, char expectedResult)
    {
        // act
        var actualResult = converter.ConvertToProviderTyped(valueToConvert);

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData('M', Sex.Male)]
    [InlineData('F', Sex.Female)]
    [InlineData('X', Sex.Unknown)]
    public void ConvertFromProviderTyped_Should_Return_ExpectedResult(char valueToConvert, Sex expectedResult)
    {
        // act
        var actualResult = converter.ConvertFromProviderTyped(valueToConvert);

        // assert
        actualResult.Should().Be(expectedResult);
    }
}
