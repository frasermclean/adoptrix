﻿using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

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
