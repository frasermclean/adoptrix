using System.Text;

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
        var id = Guid.NewGuid();

        // act
        var animal = CreateAnimal(name.Trim(), id, dateOfBirth, description, breed, sex);

        // assert
        animal.Id.Should().Be(id);
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
    public void CreateAnimal_WithLongName_ShouldThrowArgumentException()
    {
        // arrange
        var name = Enumerable.Range(0, Animal.NameMaxLength + 1)
            .Select(_ => 'x')
            .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
            .ToString();

        // act
        Action action = () => _ = CreateAnimal(name);

        // assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Name cannot exceed {Animal.NameMaxLength} characters.*");
    }

    [Fact]
    public void CreateAnimal_WithLongDescription_ShouldThrowArgumentException()
    {
        // arrange
        var description = Enumerable.Range(0, Animal.DescriptionMaxLength + 1)
            .Select(_ => 'x')
            .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
            .ToString();

        // act
        Action action = () => _ = CreateAnimal("Fluffy", description: description);

        // assert
        action.Should().Throw<ArgumentException>()
            .WithMessage($"Description cannot exceed {Animal.DescriptionMaxLength} characters.*");
    }

    [Fact]
    public void TwoAnimals_WithSameIds_Should_BeEqual()
    {
        // arrange
        var id = Guid.NewGuid();
        var max = CreateAnimal("Max", id);
        var felix = CreateAnimal("Felix", id);
        var otherObject = new object();

        // assert
        max.Id.Should().Be(id);
        max.Should().Be(felix);
        max.Equals(otherObject).Should().BeFalse();
        max.GetHashCode().Should().Be(felix.GetHashCode());
        max.Name.Should().Be("Max");
    }

    private static Animal CreateAnimal(string name, Guid? id = null, DateOnly dateOfBirth = default,
        string? description = null, Breed? breed = null, Sex sex = Sex.Male) => new(name, dateOfBirth, description)
    {
        Id = id ?? Guid.NewGuid(),
        Breed = breed ?? new Breed("Labrador") { Species = new Species("Dog") },
        Sex = sex,
    };
}
