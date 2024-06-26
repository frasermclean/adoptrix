﻿using Adoptrix.Domain.Events;

namespace Adoptrix.Application.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
