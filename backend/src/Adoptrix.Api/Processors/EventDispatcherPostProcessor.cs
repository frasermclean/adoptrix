using Adoptrix.Domain.Services;
using FastEndpoints;

namespace Adoptrix.Api.Processors;

public class EventDispatcherPostProcessor(
    ILogger<EventDispatcherPostProcessor> logger,
    IEventQueueService eventQueueService) : IGlobalPostProcessor
{
    public Task PostProcessAsync(IPostProcessorContext context, CancellationToken cancellationToken)
    {
        context.HttpContext.Response.OnCompleted(OnResponseCompleted);
        return Task.CompletedTask;
    }

    private Task OnResponseCompleted()
    {
        while (eventQueueService.HasEvents)
        {
            var domainEvent = eventQueueService.PopDomainEvent();
            if (domainEvent is null)
            {
                continue;
            }

            logger.LogInformation("Dispatching domain event {DomainEvent}", domainEvent.GetType().Name);
        }

        return Task.CompletedTask; // TODO: Potentially remove this processor
    }
}