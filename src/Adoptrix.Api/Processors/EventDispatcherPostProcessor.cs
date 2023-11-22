using Adoptrix.Domain.Services;
using FastEndpoints;
using FluentValidation.Results;

namespace Adoptrix.Api.Processors;

public class EventDispatcherPostProcessor(ILogger<EventDispatcherPostProcessor> logger,
        IEventQueueService eventQueueService)
    : IGlobalPostProcessor
{
    private readonly ILogger<EventDispatcherPostProcessor> logger = logger;
    private readonly IEventQueueService eventQueueService = eventQueueService;

    public Task PostProcessAsync(object request, object? response, HttpContext httpContext,
        IReadOnlyCollection<ValidationFailure> failures, CancellationToken cancellationToken)
    {
        httpContext.Response.OnCompleted(OnResponseCompleted);
        return Task.CompletedTask;
    }

    private async Task OnResponseCompleted()
    {
        while (eventQueueService.HasEvents)
        {
            var domainEvent = eventQueueService.PopDomainEvent();
            if (domainEvent is null)
            {
                continue;
            }

            logger.LogInformation("Dispatching domain event {DomainEvent}", domainEvent.GetType().Name);

            await domainEvent.PublishAsync();
        }
    }
}