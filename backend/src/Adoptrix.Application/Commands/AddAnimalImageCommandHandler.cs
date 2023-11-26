using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class AddAnimalImageCommandHandler(ILogger<AddAnimalImageCommandHandler> logger, IAnimalsRepository repository,
    IAnimalImageManager imageManager) : ICommandHandler<AddAnimalImageCommand, Result>
{
    public async Task<Result> ExecuteAsync(AddAnimalImageCommand command, CancellationToken cancellationToken)
    {
        var fileName = imageManager.GenerateFileName(command.Animal.Id, command.ContentType, command.OriginalFileName);

        // check if the image already exists in the model
        if (command.Animal.ImageExists(fileName))
        {
            logger.LogWarning(
                "Duplicate image detected with file name {FileName}, original file name: {OriginalFileName}",
                fileName, command.OriginalFileName);

            return new DuplicateImageError(fileName);
        }

        // upload the image to blob storage
        await imageManager.UploadImageAsync($"{command.Animal.Id}/{fileName}", command.FileStream, command.ContentType,
            cancellationToken);

        // update animal in the database
        command.Animal.AddImage(fileName, command.Description, command.OriginalFileName);
        var updateResult = await repository.UpdateAsync(command.Animal, cancellationToken);

        return updateResult.ToResult();
    }
}