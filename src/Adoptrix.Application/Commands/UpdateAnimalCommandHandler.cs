using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class UpdateAnimalCommandHandler(IAnimalsRepository repository, ILogger<UpdateAnimalCommandHandler> logger)
    : ICommandHandler<UpdateAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(UpdateAnimalCommand command, CancellationToken cancellationToken)
    {
        var getResult = await repository.GetAsync(command.Id, cancellationToken);
        if (getResult.IsFailed)
        {
            logger.LogError("Could not find animal with Id {Id} to update", command.Id);
            return getResult;
        }

        var animal = getResult.Value;

        // update properties on the animal
        animal.Name = command.Name;
        animal.Description = command.Description;
        animal.Species = command.Species;
        animal.DateOfBirth = command.DateOfBirth;

        var updateResult = await repository.UpdateAsync(animal, cancellationToken);
        if (updateResult.IsFailed)
        {
            logger.LogError("Could not update animal with Id {Id}", command.Id);
            return updateResult;
        }

        logger.LogInformation("Updated animal with id {Id}", updateResult.Value.Id);
        return updateResult;
    }
}