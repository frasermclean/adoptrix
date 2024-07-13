using Adoptrix.Tests.Shared.Factories;

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

        // act
        var animal = AnimalFactory.Create(id, name, breed, sex, dateOfBirth, imageCount, userId);

        // assert
        animal.Id.Should().Be(id);
        animal.Name.Should().Be(name);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(sex);
        animal.DateOfBirth.Should().Be(dateOfBirth);
        animal.Images.Should().HaveCount(imageCount);
        animal.CreatedBy.Should().Be(userId);
    }

    [Fact]
    public void TwoEntities_WithSameIds_Should_BeEqual()
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

    [Fact]
    public void AddImage_Should_AddImage()
    {
        // arrange
        var animal = AnimalFactory.Create(name: "Fido");
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
}
