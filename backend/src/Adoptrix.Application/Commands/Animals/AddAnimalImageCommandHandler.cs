using Adoptrix.Application.Events;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Services;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands.Animals;

public class AddAnimalImageCommandHandler(
    ILogger<AddAnimalImageCommandHandler> logger,
    IAnimalsRepository repository,
    IAnimalImageManager imageManager,
    IEventQueueService eventQueueService) : ICommandHandler<AddAnimalImageCommand, Result>
{
    public async Task<Result> ExecuteAsync(AddAnimalImageCommand command, CancellationToken cancellationToken)
    {
        // add image to animal
        var addImageResult = command.Animal.AddImage(command.FileName, command.ContentType, command.Description,
            command.UserId);
        if (addImageResult.IsFailed)
        {
            logger.LogWarning("Failed to add image to animal: {AnimalId}", command.Animal.Id);
            return addImageResult.ToResult();
        }

        // upload the original image to blob storage
        await imageManager.UploadImageAsync(command.Animal.Id, addImageResult.Value.Id, command.FileStream,
            command.ContentType, ImageCategory.Original, cancellationToken);

        // update animal in the database
        var updateResult = await repository.UpdateAsync(command.Animal, cancellationToken);
        if (updateResult.IsSuccess)
        {
            eventQueueService.PushDomainEvent(new AnimalImageAddedEvent(command.Animal.Id, addImageResult.Value.Id));
        }

        return updateResult.ToResult();
    }
}