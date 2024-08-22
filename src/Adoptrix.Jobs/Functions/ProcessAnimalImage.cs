using Adoptrix.Core.Events;
using Adoptrix.Logic.Services;
using Adoptrix.Persistence;
using Microsoft.Azure.Functions.Worker;

namespace Adoptrix.Jobs.Functions;

public class ProcessAnimalImage(IAnimalImagesManager animalImagesManager)
{
    [Function(nameof(ProcessAnimalImage))]
    public async Task ExecuteAsync([QueueTrigger(QueueNames.AnimalImageAdded)] AnimalImageAddedEvent data,
        CancellationToken cancellationToken = default)
    {
        await animalImagesManager.ProcessAnimalImageAsync(data, cancellationToken);
    }
}
