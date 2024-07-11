using Adoptrix.Core.Events;
using Adoptrix.Persistence.Services;
using Adoptrix.Persistence.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Adoptrix.Persistence.Tests.Services;

[Trait("Category", "Integration")]
public class EventPublisherTests(StorageEmulatorFixture fixture) : IClassFixture<StorageEmulatorFixture>
{
    private readonly EventPublisher eventPublisher = new(Mock.Of<ILogger<EventPublisher>>(), fixture.ServiceProvider!);

    [Fact]
    public async Task PublishDomainEventAsync_WithAnimalDeletedEvent_Should_ReturnSuccess()
    {
        // arrange
        var animalDeletedEvent = new AnimalDeletedEvent(Guid.NewGuid());

        // act
        await eventPublisher.PublishAsync(animalDeletedEvent);
    }
}
