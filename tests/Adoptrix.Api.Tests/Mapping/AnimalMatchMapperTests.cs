using Adoptrix.Api.Mapping;
using Adoptrix.Core;
using Adoptrix.Persistence.Responses;

namespace Adoptrix.Api.Tests.Mapping;

public class AnimalMatchMapperTests
{
    [Fact]
    public void ToMatch_WithNullImage_ShouldReturnExpectedResult()
    {
        // arrange
        var item = new SearchAnimalsItem
        {
            Id = 3,
            Name = "Shaggy",
            SpeciesName = "Dog",
            BreedName = "Golden Retriever",
            Sex = Sex.Male,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Today),
            Slug = "shaggy-123",
            CreatedAt = DateTime.UtcNow,
            Image = null
        };

        // act
        var match = item.ToMatch();

        // assert
        match.Id.Should().Be(item.Id);
        match.Slug.Should().Be(item.Slug);
        match.Name.Should().Be(item.Name);
        match.SpeciesName.Should().Be(item.SpeciesName);
        match.BreedName.Should().Be(item.BreedName);
        match.Image.Should().BeNull();
    }
}
