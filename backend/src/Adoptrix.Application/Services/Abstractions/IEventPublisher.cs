using Adoptrix.Domain.Events;

namespace Adoptrix.Application.Services.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
}
