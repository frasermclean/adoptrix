using Adoptrix.Application.Services;
using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Tests.Shared;
using Adoptrix.Tests.Shared.Factories;
using Microsoft.Extensions.Logging;


namespace Adoptrix.Application.Tests.Services;

public class AnimalsServiceTests
{
    private readonly Mock<IAnimalsRepository> animalsRepositoryMock = new();
    private readonly Mock<IBreedsRepository> breedsRepositoryMock = new();

    private readonly AnimalsService animalsService;

    public AnimalsServiceTests()
    {
        animalsService = new AnimalsService(Mock.Of<ILogger<AnimalsService>>(), animalsRepositoryMock.Object,
            breedsRepositoryMock.Object, Mock.Of<IAnimalImageManager>(), Mock.Of<IEventPublisher>());
    }

    [Theory, AdoptrixAutoData]
    public async Task AddAsync_WithValidRequest_ShouldReturnSuccess(AddAnimalRequest request)
    {
        // arrange
        breedsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));

        // act
        var result = await animalsService.AddAsync(request);

        // assert
        result.Should().BeSuccess();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be(request.Name);
        result.Value.Description.Should().Be(request.Description);
    }

    [Theory, AdoptrixAutoData]
    public async Task UpdateAsync_WithValidRequest_ShouldReturnSuccess(UpdateAnimalRequest request)
    {
        // arrange
        animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnimalFactory.Create(request.AnimalId));
        breedsRepositoryMock.Setup(repository => repository.GetByIdAsync(request.BreedId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BreedFactory.Create(request.BreedId));

        // act
        var result = await animalsService.UpdateAsync(request);

        // assert
        result.Should().BeSuccess();
    }
}
