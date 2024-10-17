namespace Adoptrix.Core.Tests;

public class AnimalTests
{
    [Fact]
    public void CreateAnimal_WithSpecifiedValues_ShouldReturnExpectedResult()
    {
        // arrange
        const string name = "Susie";
        const string description = "A lovely dog";
        var species = new Species("Dog");
        var breed = new Breed("Golden Retriever") { Species = species };
        const Sex sex = Sex.Female;
        var dateOfBirth = new DateOnly(2021, 2, 3);

        // act
        var animal = Animal.Create(name, description, breed, sex, dateOfBirth);

        // assert
        animal.Name.Should().Be(name);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(sex);
        animal.DateOfBirth.Should().Be(dateOfBirth);
        animal.Slug.Should().Be("susie-2021-02-03");
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

    [Theory]
    [InlineData("Buddy", 2022, 5, 2, "buddy-2022-05-02")]
    [InlineData("Mr Muffins ", 2021, 11, 30, "mr-muffins-2021-11-30")]
    [InlineData("  Fluffy ", 2020, 1, 1, "fluffy-2020-01-01")]
    public void CreateSlug_WithValidNameAndDateOfBirth_ShouldReturnExpectedResult(string name, int year, int month,
        int day, string expectedSlug)
    {
        // arrange
        var dateOfBirth = new DateOnly(year, month, day);

        // act
        var slug = Animal.CreateSlug(name, dateOfBirth);

        // assert
        slug.Should().Be(expectedSlug);
    }
}
