using System;
using Adoptrix.Client.Extensions;
using Adoptrix.Contracts.Requests;

namespace Adoptrix.Web.Tests.Extensions;

public class SearchBreedsQueryExtensionsTests
{
    [Theory]
    [InlineData(null, null, "")]
    [InlineData(null, true, "?withAnimals=True")]
    [InlineData("cdca1586-c8a8-474c-b45d-dc16a7dcce51", false, "?speciesId=cdca1586-c8a8-474c-b45d-dc16a7dcce51&withAnimals=False")]
    [InlineData("cdca1586-c8a8-474c-b45d-dc16a7dcce51", true, "?speciesId=cdca1586-c8a8-474c-b45d-dc16a7dcce51&withAnimals=True")]
    public void ToQueryString_WithSuppliedRequest_ShouldReturnExpectedQueryString(string? speciesId, bool? withAnimals,
        string expectedQueryString)
    {
        // arrange
        var request = new SearchBreedsRequest
        {
            SpeciesId = speciesId is not null ? Guid.Parse(speciesId) : null,
            WithAnimals = withAnimals,
        };

        // act
        var queryString = request.ToQueryString();

        // assert
        queryString.Should().Be(expectedQueryString);
    }
}
