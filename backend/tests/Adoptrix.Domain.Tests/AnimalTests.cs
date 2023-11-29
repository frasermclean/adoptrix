using Adoptrix.Domain.Errors;

namespace Adoptrix.Domain.Tests;

public class AnimalTests
{
    [Fact]
    public void TwoEntities_WithSameIds_Should_BeEqual()
    {
        // arrange
        var fido = CreateAnimal();
        var felix = CreateAnimal(fido.Id, "Felix");

        // assert
        fido.Should().Be(felix);
        fido.Equals(new { }).Should().BeFalse();
        fido.GetHashCode().Should().Be(felix.GetHashCode());
        fido.Name.Should().Be("Fido");
        fido.Description.Should().BeNull();
        fido.Species.Id.Should().Be(1);
        fido.Species.Name.Should().Be("Dog");
        fido.DateOfBirth.Should().Be(new DateOnly(2019, 1, 1));
    }

    [Fact]
    public void AddImage_Should_AddImage()
    {
        // arrange
        var animal = CreateAnimal();

        // act
        var result1 = animal.AddImage("abc.jpg", "A nice image", "image.jpg");
        var result2 = animal.AddImage("abc.jpg", "A nice image", "image.jpg");

        // assert
        result1.Should().BeSuccess().Which.Value.FileName.Should().Be("abc.jpg");
        animal.Images.Should().ContainSingle().Which.FileName.Should().Be("abc.jpg");
        result2.Should().BeFailure().Which.Should()
            .HaveReason<DuplicateImageError>("Image with filename abc.jpg already exists");
    }

    private static Animal CreateAnimal(int id = 1, string name = "Fido", string? description = null,
        Species? species = null, DateOnly? dateOfBirth = null) => new()
    {
        Id = id,
        Name = name,
        Description = description,
        Species = species ?? new Species { Id = 1, Name = "Dog" },
        SpeciesId = species?.Id ?? 1,
        DateOfBirth = dateOfBirth ?? new DateOnly(2019, 1, 1)
    };
}