using Adoptrix.Application.Services;
using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Tests.Shared;
using FluentResults;

namespace Adoptrix.Jobs.Tests.Functions;

public class AnimalFunctionsTests
{
    [Theory, AdoptrixAutoData]
    public async Task ProcessAnimalImage_WithValidNotification_ShouldPass(AnimalImageAddedEvent eventData,
        [Frozen] Mock<IAnimalImageManager> animalImageManagerMock, AnimalFunctions animalFunctions)
    {
        // arrange
        animalImageManagerMock.Setup(manager => manager.ProcessOriginalAsync(eventData.AnimalId, eventData.ImageId,It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // act
        await animalFunctions.ProcessAnimalImage(eventData);
    }

    [Theory, AdoptrixAutoData]
    public async Task ProcessAnimalImage_WithFailedRequest_ShouldThrow(AnimalImageAddedEvent eventData,
        [Frozen] Mock<IAnimalImageManager> animalImageManagerMock, AnimalFunctions animalFunctions)
    {
        // arrange
        animalImageManagerMock.Setup(manager => manager.ProcessOriginalAsync(eventData.AnimalId, eventData.ImageId,It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to process image"));

        // act
        var act = async () => await animalFunctions.ProcessAnimalImage(eventData);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory, AdoptrixAutoData]
    public async Task CleanupDeletedAnimal_ValidNotification_ShouldPass(AnimalDeletedEvent eventData,
        [Frozen] Mock<IAnimalImageManager> animalImageManagerMock, AnimalFunctions animalFunctions)
    {
        // arrange
        animalImageManagerMock.Setup(manager => manager.DeleteAnimalImagesAsync(eventData.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // act
        await animalFunctions.CleanupDeletedAnimal(eventData);
    }

    [Theory, AdoptrixAutoData]
    public async Task CleanupDeletedAnimal_WhenResultIsFailure_ShouldThrow(AnimalDeletedEvent eventData,
        [Frozen] Mock<IAnimalImageManager> animalImageManagerMock, AnimalFunctions animalFunctions)
    {
        // arrange
        animalImageManagerMock.Setup(manager => manager.DeleteAnimalImagesAsync(eventData.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to cleanup images"));

        // act
        var act = async () => await animalFunctions.CleanupDeletedAnimal(eventData);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }
}
