namespace Adoptrix.Core.Tests;

public class BreedTests
{
    [Fact]
    public void CreateBreed_WithValidName_ShouldReturnExpectedBreed()
    {
        // arrange
        const string name = "Labrador Retriever";
        var species = Species.Create("Dog");

        // act
        var breed = Breed.Create(name, species);

        // assert
        breed.Id.Should().Be(default);
        breed.Name.Should().Be(name);
        breed.Species.Should().Be(species);
        breed.Animals.Should().BeEmpty();
        breed.LastModifiedBy.Should().BeNull();
        breed.LastModifiedUtc.Should().Be(default);
    }

    [Fact]
    public void CreateBreed_WithLongName_ShouldThrowArgumentException()
    {
        // arrange
        const string name = "An extraordinarily long name for a breed";

        // act
        Action act = () => Breed.Create(name);

        // assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Name cannot exceed {Breed.NameMaxLength} characters.*");
    }
}
