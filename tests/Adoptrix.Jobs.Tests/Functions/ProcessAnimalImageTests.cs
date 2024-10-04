using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using Adoptrix.Tests.Shared;
using FluentResults;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests
{
    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(AnimalImageAddedEvent data,
        [Frozen] Mock<IAnimalImagesManager> animalImagesManagerMock, ProcessAnimalImage function)
    {
        // arrange
        animalImagesManagerMock.Setup(manager =>
                manager.ProcessOriginalAsync(It.IsAny<AnimalImageAddedEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        // act
        await function.ExecuteAsync(data);

        // assert
        animalImagesManagerMock.Verify(
            manager => manager.ProcessOriginalAsync(It.IsAny<AnimalImageAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithInvalidAnimalSlug_ShouldThrowException(AnimalImageAddedEvent data,
        [Frozen] Mock<IAnimalImagesManager> animalImagesManagerMock, ProcessAnimalImage function)
    {
        // arrange
        animalImagesManagerMock.Setup(manager =>
                manager.ProcessOriginalAsync(It.IsAny<AnimalImageAddedEvent>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnimalNotFoundError(data.AnimalSlug));

        // act
        var act = async () => await function.ExecuteAsync(data);

        // assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        animalImagesManagerMock.Verify(
            manager => manager.ProcessOriginalAsync(It.IsAny<AnimalImageAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
