using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands.Breeds;

public class AddBreedCommandHandler(
        ILogger<AddBreedCommandHandler> logger,
        ISpeciesRepository speciesRepository,
        IBreedsRepository breedsRepository)
    : ICommandHandler<AddBreedCommand, Result<Breed>>
{
    public async Task<Result<Breed>> ExecuteAsync(AddBreedCommand command, CancellationToken cancellationToken)
    {
        // find species
        var speciesResult = await speciesRepository.GetByNameAsync(command.Species, cancellationToken);
        if (speciesResult.IsFailed)
        {
            logger.LogError("Could not find species with name: {SpeciesName}", command.Species);
            return speciesResult.ToResult();
        }

        // check if breed already exists
        var existingBreedResult = await breedsRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingBreedResult.IsSuccess)
        {
            return new BreedConflictError(command.Name);
        }

        return await breedsRepository.AddAsync(new Breed
        {
            Name = command.Name,
            Species = speciesResult.Value,
            CreatedBy = command.UserId
        }, cancellationToken);
    }
}