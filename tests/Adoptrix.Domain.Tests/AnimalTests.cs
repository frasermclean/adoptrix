namespace Adoptrix.Domain.Tests;

public class AnimalTests
{
    [Fact]
    public void TwoEntities_WithSameIds_Should_BeEqual()
    {
        // arrange
        var fido = new Animal
        {
            Id = Guid.Parse("01a9e617-ebb1-4595-8b0d-1e12e5b38f81"),
            Name = "Fido",
            Species = Species.Dog,
            DateOfBirth = new DateOnly(2019, 1, 1)
        };
        var felix = new Animal
        {
            Id = Guid.Parse("01a9e617-ebb1-4595-8b0d-1e12e5b38f81"),
            Name = "Felix",
            Species = Species.Cat,
            DateOfBirth = new DateOnly(2017, 3, 1)
        };

        // assert
        fido.Should().Be(felix);
        fido.Equals(new { }).Should().BeFalse();
        fido.GetHashCode().Should().Be(felix.GetHashCode());
        fido.Name.Should().Be("Fido");
        fido.Species.Should().Be(Species.Dog);
        fido.DateOfBirth.Should().Be(new DateOnly(2019, 1, 1));
    }
}