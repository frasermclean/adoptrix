using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Persistence.Services;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Jobs.Tests.Functions;

public class CleanupDeletedAnimalTests
{
    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(AnimalDeletedEvent eventData,
        string[] blobNames, [Frozen] Mock<IBlobContainerManager> blobContainerManagerMock,
        CleanupDeletedAnimal cleanupDeletedAnimal)
    {
        // arrange
        blobContainerManagerMock
            .Setup(manager => manager.GetBlobNamesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(blobNames);

        // act
        await cleanupDeletedAnimal.ExecuteAsync(eventData);

        // assert
        blobContainerManagerMock.Verify(
            manager => manager.GetBlobNamesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        blobContainerManagerMock.Verify(
            manager => manager.DeleteBlobAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(blobNames.Length));
    }
}
