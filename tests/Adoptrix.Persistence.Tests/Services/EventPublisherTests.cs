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
        const string animalSlug = "bruno-2022-01-01";
        var animalDeletedEvent = new AnimalDeletedEvent(animalSlug);

        // act
        var messageId = await eventPublisher.PublishAsync(animalDeletedEvent);

        // assert
        var response = await fixture.AnimalDeletedQueueClient.PeekMessageAsync();
        response.Value.MessageId.Should().Be(messageId);
        response.Value.MessageText.Should().Contain(animalSlug);
    }

    [Fact]
    public async Task PublishAsync_WithAnimalImageAddedEvent_ShouldReturnSuccess()
    {
        // arrange
        var animalSlug = Guid.NewGuid().ToString();
        var imageId = Random.Shared.Next();
        var blobName = $"{animalSlug}/image.jpg";
        var animalImageAddedEvent = new AnimalImageAddedEvent(animalSlug, imageId, blobName);

        // act
        var messageId = await eventPublisher.PublishAsync(animalImageAddedEvent);

        // assert
        var response = await fixture.AnimalImageAddedQueueClient.PeekMessageAsync();
        response.Value.MessageId.Should().Be(messageId);
        response.Value.MessageText.Should().Contain(animalSlug);
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
