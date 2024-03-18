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
        fido.Equals(new
        {
        }).Should().BeFalse();
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
        const string fileName = "DSC0001.jpg";
        const string contentType = "image/jpeg";
        const string description = "Fido in the park";
        var userId = Guid.NewGuid();

        // act
        var image = animal.AddImage(fileName, contentType, description, userId);
        var action = () => animal.AddImage(fileName, contentType);

        // assert
        image.OriginalFileName.Should().Be(fileName);
        image.OriginalContentType.Should().Be(contentType);
        image.Description.Should().Be(description);
        image.UploadedBy.Should().Be(userId);
        animal.Images.Should().ContainSingle().Which.OriginalFileName.Should().Be(fileName);
        action.Should().Throw<ArgumentException>().WithMessage("Image with the same name already exists*");
    }

    private static Animal CreateAnimal(Guid? id = null, string name = "Fido", string? description = null,
        Species? species = null, Breed? breed = null, Sex sex = default, DateOnly? dateOfBirth = null)
    {
        species ??= new Species
        {
            Id = Guid.NewGuid(), Name = "Dog"
        };

        breed ??= new Breed
        {
            Id = Guid.NewGuid(), Name = "Golden Retriever", Species = species
        };

        return new Animal
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Description = description,
            Species = species,
            Breed = breed,
            Sex = sex,
            DateOfBirth = dateOfBirth ?? new DateOnly(2019, 1, 1)
        };
    }
}
