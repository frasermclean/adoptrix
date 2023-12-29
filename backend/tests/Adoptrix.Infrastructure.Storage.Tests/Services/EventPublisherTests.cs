using Adoptrix.Domain.Events;
using Adoptrix.Infrastructure.Storage.Services;
using Adoptrix.Infrastructure.Storage.Tests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Infrastructure.Storage.Tests.Services;

[Trait("Category", "Integration")]
public class EventPublisherTests(StorageEmulatorFixture fixture) : IClassFixture<StorageEmulatorFixture>
{
    private readonly EventPublisher eventPublisher = new(Mock.Of<ILogger<EventPublisher>>(),
        fixture.AnimalDeletedQueueClient!, fixture.AnimalImageAddedQueueClient!);

    [Fact]
    public async Task PublishDomainEventAsync_WithAnimalDeletedEvent_Should_ReturnSuccess()
    {
        // arrange
        var animalDeletedEvent = new AnimalDeletedEvent(Guid.NewGuid());

        // act
        var result = await eventPublisher.PublishDomainEventAsync(animalDeletedEvent);

        // assert
        result.Should().BeSuccess();
    }
}