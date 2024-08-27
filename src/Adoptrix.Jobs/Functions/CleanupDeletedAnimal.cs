using Adoptrix.Core.Events;
using Adoptrix.Logic.Services;
using Adoptrix.Persistence;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Jobs.Functions;

public class CleanupDeletedAnimal(IAnimalImagesManager animalImagesManager)
{
    [Function(nameof(CleanupDeletedAnimal))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalDeleted)] AnimalDeletedEvent data,
        CancellationToken cancellationToken = default)
    {
        await animalImagesManager.DeleteImagesAsync(data, cancellationToken);
    }
}
