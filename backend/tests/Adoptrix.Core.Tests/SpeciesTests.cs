namespace Adoptrix.Core.Tests;

public class SpeciesTests
{
    [Fact]
    public void NewSpecies_WithValidName_ShouldReturnExpectedSpecies()
    {
        // arrange
        const string name = "Dog";

        // act
        var species = new Species(name);

        // assert
        species.Id.Should().Be(default);
        species.Name.Should().Be(name);
        species.Breeds.Should().BeEmpty();
        species.LastModifiedBy.Should().BeNull();
        species.LastModifiedUtc.Should().Be(default);
    }

    [Fact]
    public void NewSpecies_WithLongName_ShouldThrowArgumentException()
    {
        // arrange
        const string name = "A very long name for a species";

        // act
        Action act = () => _ = new Species(name);

        // assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Name cannot exceed {Species.NameMaxLength} characters.*");
    }

    [Fact]
    public void SettingLastModifiedProperties_WithValidValues_ShouldUpdateSpecies()
    {
        // act
        var species = new Species("Bird")
        {
            LastModifiedBy = Guid.NewGuid(),
            LastModifiedUtc = DateTime.UtcNow
        };

        // assert
        species.LastModifiedBy.Should().NotBeNull();
        species.LastModifiedUtc.Should().NotBe(default);
    }
}
