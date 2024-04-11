using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Notifications.Animals;
using Adoptrix.Jobs.Functions;
using Adoptrix.Tests.Shared;
using FluentResults;
using MediatR;

namespace Adoptrix.Jobs.Tests.Functions;

public class AnimalFunctionsTests
{

    [Theory, MoqAutoData]
    public async Task ProcessAnimalImage_WithValidNotification_ShouldPass(AnimalImageAddedNotification notification,
        AnimalFunctions animalFunctions)
    {
        // act
        await animalFunctions.ProcessAnimalImage(notification);
    }

    [Theory, MoqAutoData]
    public async Task ProcessAnimalImage_WithFailedRequest_ShouldThrow(AnimalImageAddedNotification notification,
        [Frozen] Mock<ISender> senderMock, AnimalFunctions animalFunctions)
    {
        // arrange
        senderMock.Setup(sender => sender.Send(It.IsAny<ProcessAnimalImageRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to process image"));

        // act
        var act = async () => await animalFunctions.ProcessAnimalImage(notification);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory, MoqAutoData]
    public async Task CleanupDeletedAnimal_ValidNotification_ShouldPass(AnimalDeletedNotification notification,
        AnimalFunctions animalFunctions)
    {
        // act
        await animalFunctions.CleanupDeletedAnimal(notification);
    }

    [Theory, MoqAutoData]
    public async Task CleanupDeletedAnimal_WhenResultIsFailure_ShouldThrow(AnimalDeletedNotification notification,
        [Frozen] Mock<ISender> senderMock, AnimalFunctions animalFunctions)
    {
        // arrange
        senderMock.Setup(sender => sender.Send(It.IsAny<CleanupAnimalImagesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failure"));

        // act
        var act = async () => await animalFunctions.CleanupDeletedAnimal(notification);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }
}
