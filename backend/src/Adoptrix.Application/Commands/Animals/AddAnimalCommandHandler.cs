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
        var speciesResult = await speciesRepository.GetByNameAsync(command.SpeciesName, cancellationToken);
        if (speciesResult.IsFailed)
        {
            logger.LogError("Could not find species with name: {SpeciesName}", command.SpeciesName);
            return speciesResult.ToResult();
        }

        // find breed if breed name was specified
        var breedResult = command.BreedName is not null
            ? await breedsRepository.GetByNameAsync(command.BreedName, cancellationToken)
            : null;
        if (breedResult?.IsFailed ?? false)
        {
            logger.LogError("Could not find breed with name: {BreedName}", command.BreedName);
            return breedResult.ToResult();
        }

        var result = await repository.AddAsync(new Animal
        {
            Name = command.Name,
            Description = command.Description,
            Species = speciesResult.Value,
            Breed = breedResult?.Value,
            Sex = command.Sex,
            DateOfBirth = command.DateOfBirth,
            CreatedBy = command.UserId
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