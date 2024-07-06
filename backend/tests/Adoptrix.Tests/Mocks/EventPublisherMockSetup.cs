using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Events;

namespace Adoptrix.Tests.Mocks;

public static class EventPublisherMockSetup
{
    public static Mock<IEventPublisher> SetupDefaults(this Mock<IEventPublisher> mock)
    {
        mock.Setup(eventPublisher =>
                eventPublisher.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        return mock;
    }
}
