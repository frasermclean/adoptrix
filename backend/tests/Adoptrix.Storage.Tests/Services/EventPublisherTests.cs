using Adoptrix.Core.Events;
using Adoptrix.Storage.Services;
using Adoptrix.Storage.Tests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Storage.Tests.Services;

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
