namespace Adoptrix.Core.Tests;

public class AnimalTests
{
    [Theory]
    [InlineData("Buddy", "A lovely dog", "2022-05-02", "Buddy", "buddy-2022-05-02")]
    [InlineData("Mr Muffins ", null, "2021-11-30", "Mr Muffins", "mr-muffins-2021-11-30")]
    [InlineData("  Fluffy ", "Adorable kitty", "2020-01-01", "Fluffy", "fluffy-2020-01-01")]
    public void CreateAnimal_WithSpecifiedValues_ShouldReturnExpectedResult(string name, string? description,
        string dateOfBirthString, string expectedName, string expectedSlug)
    {
        // arrange
        var species = new Species("Dog");
        var breed = new Breed("Golden Retriever") { Species = species };
        var sex = Random.Shared.Next(2) == 1 ? Sex.Male : Sex.Female;
        var dateOfBirth = DateOnly.Parse(dateOfBirthString);

        // act
        var animal = Animal.Create(name, description, breed, sex, dateOfBirth);

        // assert
        animal.Id.Should().BeEmpty();
        animal.Name.Should().Be(expectedName);
        animal.Description.Should().Be(description);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(sex);
        animal.DateOfBirth.Should().Be(dateOfBirth);
        animal.Slug.Should().Be(expectedSlug);
        animal.Images.Should().BeEmpty();
        animal.LastModifiedBy.Should().BeNull();
        animal.LastModifiedUtc.Should().Be(default);
    }

    [Fact]
    public void TwoAnimals_WithSameIds_Should_BeEqual()
    {
        // arrange
        var max = Animal.Create("Max");
        var felix = Animal.Create("Felix");
        var otherObject = new object();

        // assert
        max.Id.Should().BeEmpty();
        max.Should().Be(felix);
        max.Equals(otherObject).Should().BeFalse();
        max.GetHashCode().Should().Be(felix.GetHashCode());
        max.Name.Should().Be("Max");
    }
}
