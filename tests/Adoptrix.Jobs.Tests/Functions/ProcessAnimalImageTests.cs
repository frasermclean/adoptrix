using Adoptrix.Core.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Logic.Services;
using Adoptrix.Tests.Shared;

namespace Adoptrix.Jobs.Tests.Functions;

public class ProcessAnimalImageTests
{
    private readonly Mock<IAnimalImagesManager> animalImagesManagerMock = new();
    private readonly ProcessAnimalImage function;

    public ProcessAnimalImageTests()
    {
        function = new ProcessAnimalImage(animalImagesManagerMock.Object);
    }

    [Theory, AdoptrixAutoData]
    public async Task ExecuteAsync_WithValidEventData_ShouldPass(AnimalImageAddedEvent data)
    {
        // arrange
        animalImagesManagerMock.Setup(manager =>
                manager.ProcessAnimalImageAsync(It.IsAny<AnimalImageAddedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // act
        await function.ExecuteAsync(data);
    }
}
