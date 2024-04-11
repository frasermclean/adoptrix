using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Handlers.Animals;

public class CleanupAnimalImagesHandler(ILogger<CleanupAnimalImagesHandler> logger, IAnimalImageManager imageManager)
    : IRequestHandler<CleanupAnimalImagesRequest, Result>
{
    public async Task<Result> Handle(CleanupAnimalImagesRequest request, CancellationToken cancellationToken)
    {
        var result = await imageManager.DeleteAnimalImagesAsync(request.AnimalId, cancellationToken);
        if (result.IsFailed)
        {
            logger.LogError("Failed to delete images for animal {AnimalId}", request.AnimalId);
            return result.ToResult();
        }

        logger.LogInformation("Deleted {Count} images for animal {AnimalId}", result.Value, request.AnimalId);
        return Result.Ok();
    }
}
