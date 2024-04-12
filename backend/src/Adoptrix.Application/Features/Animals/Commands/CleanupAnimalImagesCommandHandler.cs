using Adoptrix.Application.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Features.Animals.Commands;

public class CleanupAnimalImagesCommandHandler(ILogger<CleanupAnimalImagesCommandHandler> logger, IAnimalImageManager imageManager)
    : IRequestHandler<CleanupAnimalImagesCommand, Result>
{
    public async Task<Result> Handle(CleanupAnimalImagesCommand command, CancellationToken cancellationToken)
    {
        var result = await imageManager.DeleteAnimalImagesAsync(command.AnimalId, cancellationToken);
        if (result.IsFailed)
        {
            logger.LogError("Failed to delete images for animal {AnimalId}", command.AnimalId);
            return result.ToResult();
        }

        logger.LogInformation("Deleted {Count} images for animal {AnimalId}", result.Value, command.AnimalId);
        return Result.Ok();
    }
}
