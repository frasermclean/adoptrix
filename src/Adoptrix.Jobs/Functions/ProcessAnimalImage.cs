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
        var result = await animalImagesManager.ProcessOriginalAsync(data, cancellationToken);

        if (result.IsFailed)
        {
            throw new InvalidOperationException(result.Errors.First().Message);
        }
    }
}
