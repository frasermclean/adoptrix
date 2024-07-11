using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core;
using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Jobs.Services;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests
{
    [Theory(Skip = "Not currently working"), AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(ProcessAnimalImage sut, Animal animal,
        [Frozen] Mock<IAnimalsRepository> animalsRepositoryMock,
        [Frozen] Mock<IImageProcessor> imageProcessorMock,
        ImageStreamBundle bundle)
    {
        // arrange
        var data = new AnimalImageAddedEvent(animal.Id, animal.Images.First().Id);
        animalsRepositoryMock.Setup(repository => repository.GetByIdAsync(data.AnimalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(animal);
        imageProcessorMock.Setup(processor =>
                processor.ProcessOriginalAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bundle);

        // act
        await sut.ExecuteAsync(data);
    }
}
