using Adoptrix.Domain.Events;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IEventPublisher
{
    Task<Result> PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent;
}