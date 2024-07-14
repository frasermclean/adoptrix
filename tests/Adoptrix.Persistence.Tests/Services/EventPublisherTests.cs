using Adoptrix.Core.Events;
using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(StorageEmulatorCollection))]
public class EventPublisherTests(StorageEmulatorFixture fixture)
{
    private readonly EventPublisher eventPublisher = new(Mock.Of<ILogger<EventPublisher>>(), fixture.ServiceProvider);

    [Fact]
    public async Task PublishAsync_WithAnimalDeletedEvent_ShouldReturnSuccess()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var animalDeletedEvent = new AnimalDeletedEvent(animalId);

        // act
        var messageId = await eventPublisher.PublishAsync(animalDeletedEvent);

        // assert
        var response = await fixture.AnimalDeletedQueueClient.PeekMessageAsync();
        response.Value.MessageId.Should().Be(messageId);
        response.Value.MessageText.Should().Contain(animalId.ToString());
    }

    [Fact]
    public async Task PublishAsync_WithAnimalImageAddedEvent_ShouldReturnSuccess()
    {
        // arrange
        var animalId = Guid.NewGuid();
        var imageId = Guid.NewGuid();
        var blobName = $"{animalId}/image.jpg";
        var animalImageAddedEvent = new AnimalImageAddedEvent(animalId, imageId, blobName);

        // act
        var messageId = await eventPublisher.PublishAsync(animalImageAddedEvent);

        // assert
        var response = await fixture.AnimalImageAddedQueueClient.PeekMessageAsync();
        response.Value.MessageId.Should().Be(messageId);
        response.Value.MessageText.Should().Contain(animalId.ToString());
        response.Value.MessageText.Should().Contain(imageId.ToString());
        response.Value.MessageText.Should().Contain(blobName);
    }

    [Fact]
    public async Task PublishAsync_WithInvalidEvent_ShouldThrowException()
    {
        // arrange
        var invalidEvent = new InvalidEvent();

        // act
        Func<Task> act = () => eventPublisher.PublishAsync(invalidEvent);

        // assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    private class InvalidEvent : IDomainEvent { }
}
