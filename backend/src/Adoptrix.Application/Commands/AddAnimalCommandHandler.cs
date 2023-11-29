using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class AddAnimalCommandHandler(
        ILogger<AddAnimalCommandHandler> logger,
        IAnimalsRepository repository,
        ISpeciesRepository speciesRepository)
    : ICommandHandler<AddAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var speciesResult = await speciesRepository.GetSpeciesByNameAsync(command.SpeciesName, cancellationToken);
        if (speciesResult.IsFailed)
        {
            logger.LogError("Could not find species with name: {Name}", command.SpeciesName);
            return speciesResult.ToResult();
        }

        var result = await repository.AddAsync(new Animal
        {
            Name = command.Name,
            Description = command.Description,
            Species = speciesResult.Value,
            DateOfBirth = command.DateOfBirth
        }, cancellationToken);

        if (result.IsFailed)
        {
            logger.LogError("Could not add animal with name {Name}", command.Name);
            return result;
        }

        logger.LogInformation("Added animal with id {Id}", result.Value.Id);
        return result;
    }
}