namespace Adoptrix.Core.Tests;

public class BreedTests
{
    [Fact]
    public void NewBreed_WithValidName_ShouldReturnExpectedBreed()
    {
        // arrange
        const string name = "Labrador Retriever";
        var species = new Species("Dog");

        // act
        var breed = new Breed(name) { Species = species };

        // assert
        breed.Id.Should().Be(default);
        breed.Name.Should().Be(name);
        breed.Species.Should().Be(species);
        breed.Animals.Should().BeEmpty();
        breed.LastModifiedBy.Should().BeNull();
        breed.LastModifiedUtc.Should().Be(default);
    }

    [Fact]
    public void NewBreed_WithLongName_ShouldThrowArgumentException()
    {
        // arrange
        const string name = "An extraordinarily long name for a breed";
        var species = new Species("Dog");

        // act
        Action act = () => _ = new Breed(name) { Species = species };

        // assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Name cannot exceed {Breed.NameMaxLength} characters.*");
    }
}
