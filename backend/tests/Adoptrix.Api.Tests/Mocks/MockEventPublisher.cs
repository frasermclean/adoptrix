using Adoptrix.Application.Services;
using Adoptrix.Domain.Events;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockEventPublisher : IEventPublisher
{
    public Task<Result> PublishDomainEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        return Task.FromResult(Result.Ok());
    }
}