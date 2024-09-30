using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Jobs.Tests.Functions;

public class CleanupDeletedAnimalTests
{
    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(AnimalDeletedEvent eventData,
        [Frozen] Mock<IAnimalImagesManager> animalImagesManagerMock,
        CleanupDeletedAnimal function)
    {
        // act
        await function.ExecuteAsync(eventData);

        // assert
        animalImagesManagerMock.Verify(
            manager => manager.DeleteImagesAsync(It.IsAny<AnimalDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
