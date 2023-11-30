using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands.Animals;

public class UpdateAnimalCommandHandler(
        ISqidConverter sqidConverter,
        IAnimalsRepository repository,
        ILogger<UpdateAnimalCommandHandler> logger)
    : ICommandHandler<UpdateAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(UpdateAnimalCommand command, CancellationToken cancellationToken)
    {
        var animalId = sqidConverter.ConvertToInt(command.Id);
        var getResult = await repository.GetAsync(animalId, cancellationToken);
        if (getResult.IsFailed)
        {
            logger.LogError("Could not find animal with Id {Id} to update", animalId);
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
            logger.LogError("Could not update animal with Id {Id}", animalId);
            return updateResult;
        }

        logger.LogInformation("Updated animal with id {Id}", updateResult.Value.Id);
        return updateResult;
    }
}