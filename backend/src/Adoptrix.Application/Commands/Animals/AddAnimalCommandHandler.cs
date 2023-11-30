using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands.Animals;

public class AddAnimalCommandHandler(
        ILogger<AddAnimalCommandHandler> logger,
        IAnimalsRepository repository,
        ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository)
    : ICommandHandler<AddAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        // find species
        var speciesResult = await speciesRepository.GetSpeciesByNameAsync(command.Species, cancellationToken);
        if (speciesResult.IsFailed)
        {
            logger.LogError("Could not find species with name: {Species}", command.Species);
            return speciesResult.ToResult();
        }

        // find breed if breed name was specified
        var breedResult = command.Breed is not null
            ? await breedsRepository.GetBreedByNameAsync(command.Breed, cancellationToken)
            : null;
        if (breedResult?.IsFailed ?? false)
        {
            logger.LogError("Could not find breed with name: {Breed}", command.Breed);
            return breedResult.ToResult();
        }

        var result = await repository.AddAsync(new Animal
        {
            Name = command.Name,
            Description = command.Description,
            Species = speciesResult.Value,
            Breed = breedResult?.Value,
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