using Adoptrix.Core;
using Adoptrix.Core.Abstractions;
using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Services;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests
{
    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(Animal animal, ImageStreamBundle bundle,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IBlobContainerManager> blobContainerManagerMock,
        [Frozen] Mock<IImageProcessor> imageProcessorMock,
        ProcessAnimalImage function)
    {
        // arrange
        var imageId = animal.Images.First().Id;
        var data = new AnimalImageAddedEvent(animal.Id, imageId);
        animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);
        imageProcessorMock.Setup(processor =>
                processor.ProcessOriginalAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bundle);

        // act
        await function.ExecuteAsync(data);

        // assert
        animalsRepositoryMock.Verify(repository => repository.GetByIdAsync(animal.Id, It.IsAny<CancellationToken>()),
            Times.Once);
        blobContainerManagerMock.Verify(manager =>
            manager.OpenReadStreamAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        imageProcessorMock.Verify(processor => processor.ProcessOriginalAsync(It.IsAny<Stream>(),
            It.IsAny<CancellationToken>()), Times.Once);
        blobContainerManagerMock.Verify(manager =>
            manager.UploadBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Exactly(3));
        animalsRepositoryMock.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithInvalidAnimalId_ShouldThrowException(AnimalImageAddedEvent data,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock, ProcessAnimalImage function)
    {
        // arrange
        animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Animal);

        // act
        var act = async () => await function.ExecuteAsync(data);

        // assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        animalsRepositoryMock.Verify(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
