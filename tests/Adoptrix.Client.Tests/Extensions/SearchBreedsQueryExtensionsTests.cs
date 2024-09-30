using Adoptrix.Client.Extensions;
using Adoptrix.Core.Requests;

namespace Adoptrix.Client.Tests.Extensions;

public class SearchBreedsQueryExtensionsTests
{
    [Theory]
    [InlineData(null, null, "")]
    [InlineData(null, true, "?withAnimals=True")]
    [InlineData("Dog", false, "?speciesName=Dog&withAnimals=False")]
    [InlineData("Cat", true, "?speciesName=Cat&withAnimals=True")]
    public void ToQueryString_WithSuppliedRequest_ShouldReturnExpectedQueryString(string? speciesName, bool? withAnimals,
        string expectedQueryString)
    {
        // arrange
        var request = new SearchBreedsRequest
        {
            SpeciesName = speciesName,
            WithAnimals = withAnimals,
        };

        // act
        var queryString = request.ToQueryString();

        // assert
        queryString.Should().Be(expectedQueryString);
    }
}
