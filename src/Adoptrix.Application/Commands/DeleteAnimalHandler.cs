using Adoptrix.Application.Services.Repositories;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class DeleteAnimalHandler(ILogger<DeleteAnimalHandler> logger, IAnimalsRepository repository)
    : ICommandHandler<DeleteAnimalCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        // find existing animal
        var animalResult = await repository.GetAsync(command.Id, cancellationToken);
        if (animalResult.IsFailed)
        {
            logger.LogError("Could not delete animal with id {Id}", command.Id);
            return animalResult.ToResult();
        }

        await repository.DeleteAsync(animalResult.Value, cancellationToken);

        logger.LogInformation("Deleted animal with id {Id}", command.Id);
        return Result.Ok();
    }
}