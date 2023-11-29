using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Application.Commands;

public class AddAnimalCommandHandler(IAnimalsRepository repository, ILogger<AddAnimalCommandHandler> logger)
    : ICommandHandler<AddAnimalCommand, Result<Animal>>
{
    public async Task<Result<Animal>> ExecuteAsync(AddAnimalCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.AddAsync(new Animal
        {
            Name = command.Name,
            Description = command.Description,
            SpeciesId = command.SpeciesId,
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