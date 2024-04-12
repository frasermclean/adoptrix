using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Domain.Events;
using Adoptrix.Jobs.Functions;
using Adoptrix.Tests.Shared;
using FluentResults;
using MediatR;

namespace Adoptrix.Jobs.Tests.Functions;

public class AnimalFunctionsTests
{
    [Theory, AdoptrixAutoData]
    public async Task ProcessAnimalImage_WithValidNotification_ShouldPass(AnimalImageAddedEvent eventData,
        AnimalFunctions animalFunctions)
    {
        // act
        await animalFunctions.ProcessAnimalImage(eventData);
    }

    [Theory, AdoptrixAutoData]
    public async Task ProcessAnimalImage_WithFailedRequest_ShouldThrow(AnimalImageAddedEvent eventData,
        [Frozen] Mock<ISender> senderMock, AnimalFunctions animalFunctions)
    {
        // arrange
        senderMock.Setup(sender => sender.Send(It.IsAny<ProcessAnimalImageCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failed to process image"));

        // act
        var act = async () => await animalFunctions.ProcessAnimalImage(eventData);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory, AdoptrixAutoData]
    public async Task CleanupDeletedAnimal_ValidNotification_ShouldPass(AnimalDeletedEvent eventData,
        AnimalFunctions animalFunctions)
    {
        // act
        await animalFunctions.CleanupDeletedAnimal(eventData);
    }

    [Theory, AdoptrixAutoData]
    public async Task CleanupDeletedAnimal_WhenResultIsFailure_ShouldThrow(AnimalDeletedEvent eventData,
        [Frozen] Mock<ISender> senderMock, AnimalFunctions animalFunctions)
    {
        // arrange
        senderMock.Setup(sender => sender.Send(It.IsAny<CleanupAnimalImagesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Failure"));

        // act
        var act = async () => await animalFunctions.CleanupDeletedAnimal(eventData);

        // assert
        await act.Should().ThrowAsync<Exception>();
    }
}
