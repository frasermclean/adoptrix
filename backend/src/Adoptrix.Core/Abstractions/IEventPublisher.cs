using Adoptrix.Core.Events;

namespace Adoptrix.Core.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
