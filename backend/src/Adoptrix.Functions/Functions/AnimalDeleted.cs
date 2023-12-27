using Adoptrix.Domain.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Functions.Functions;

public class AnimalDeleted(ILogger<AnimalDeleted> logger)
{
    [Function(nameof(AnimalDeleted))]
    public void Run([QueueTrigger("animal-deleted")] AnimalDeletedEvent eventData)
    {
        logger.LogInformation("Animal deleted event. Animal ID : {AnimalId}", eventData.AnimalId);
    }
}