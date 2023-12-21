using Adoptrix.Domain.Errors;

namespace Adoptrix.Domain.Tests;

public class AnimalTests
{
    [Fact]
    public void TwoEntities_WithSameIds_Should_BeEqual()
    {
        // arrange
        var id = Guid.NewGuid();
        var fido = CreateAnimal(id);
        var felix = CreateAnimal(id, "Felix");

        // assert
        fido.Should().Be(felix);
        fido.Equals(new { }).Should().BeFalse();
        fido.GetHashCode().Should().Be(felix.GetHashCode());
        fido.Id.Should().Be(id);
        fido.Name.Should().Be("Fido");
        fido.Description.Should().BeNull();
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

    private static Animal CreateAnimal(Guid? id = null, string name = "Fido", string? description = null,
        Species? species = null, DateOnly? dateOfBirth = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name,
        Description = description,
        Species = species ?? new Species { Id = Guid.NewGuid(), Name = "Dog" },
        DateOfBirth = dateOfBirth ?? new DateOnly(2019, 1, 1)
    };
}