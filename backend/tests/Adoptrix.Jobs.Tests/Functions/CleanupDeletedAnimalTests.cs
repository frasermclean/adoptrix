using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Persistence.Services;

namespace Adoptrix.Jobs.Tests.Functions;

public class CleanupDeletedAnimalTests
{
    [Theory, AutoMoqData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(AnimalDeletedEvent eventData,
        [Frozen] Mock<IBlobContainerManager> animalImagesManagerMock,
        CleanupDeletedAnimal function)
    {
        // arrange
        var blobNames = new[] { "image1.jpg", "image2.jpg", "image3.jpg" };
        animalImagesManagerMock
            .Setup(manager => manager.GetBlobNamesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(blobNames);

        // act
        await function.ExecuteAsync(eventData);

        // assert
        animalImagesManagerMock.Verify(
            manager => manager.DeleteBlobAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Exactly(blobNames.Length));
    }
}
