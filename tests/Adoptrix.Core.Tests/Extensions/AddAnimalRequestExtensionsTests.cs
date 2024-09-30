using Adoptrix.Core.Extensions;
using Adoptrix.Core.Requests;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Core.Tests.Extensions;

public class AddAnimalRequestExtensionsTests
{
    [Fact]
    public void ToAnimal_WithValidRequest_ShouldReturnExpectedAnimal()
    {
        // arrange
        var request = new AddAnimalRequest
        {
            Name = "Sasha",
            Description = "A cute cat",
            Sex = Sex.Female,
            DateOfBirth = new DateOnly(2022, 1, 2),
            UserId = Guid.NewGuid()
        };
        var breed = BreedFactory.Create();

        // act
        var animal = request.ToAnimal(breed);

        // assert
        animal.Should().NotBeNull();
        animal.Name.Should().Be(request.Name);
        animal.Description.Should().Be(request.Description);
        animal.Breed.Should().Be(breed);
        animal.Sex.Should().Be(request.Sex);
        animal.DateOfBirth.Should().Be(request.DateOfBirth);
        animal.Slug.Should().NotBeEmpty();
        animal.LastModifiedBy.Should().Be(request.UserId);
    }
}
