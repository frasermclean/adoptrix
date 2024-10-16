using Adoptrix.Core.Factories;

namespace Adoptrix.Core.Tests;

public class AnimalTests
{
    [Fact]
    public void CreateAnimal_WithSpecifiedValues_ShouldReturnExpectedResult()
    {
        // arrange
        var id = Guid.NewGuid();
        const string name = "Susie";
        var breed = BreedFactory.Create();
        const Sex sex = Sex.Female;
        var userId = Guid.NewGuid();
        const int imageCount = 3;
        var dateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow - TimeSpan.FromDays(365 * 1.5));
        var slug = Animal.CreateSlug(name, dateOfBirth);

        // act
        var animal = AnimalFactory.Create(id, name, breed, sex, dateOfBirth, slug, imageCount, userId);

        // assert
        animal.Id.Should().Be(id);
        animal.Name.Should().Be(name);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(sex);
        animal.DateOfBirth.Should().Be(dateOfBirth);
        animal.Slug.Should().Be(slug);
        animal.Images.Should().HaveCount(imageCount);
        animal.LastModifiedBy.Should().Be(userId);
    }

    [Fact]
    public void TwoAnimals_WithSameIds_Should_BeEqual()
    {
        // arrange
        var id = Guid.NewGuid();
        var max = AnimalFactory.Create(id, "Max");
        var felix = AnimalFactory.Create(id, "Felix");
        var otherObject = new object();

        // assert
        max.Should().Be(felix);
        max.Equals(otherObject).Should().BeFalse();
        max.GetHashCode().Should().Be(felix.GetHashCode());
        max.Id.Should().Be(id);
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
