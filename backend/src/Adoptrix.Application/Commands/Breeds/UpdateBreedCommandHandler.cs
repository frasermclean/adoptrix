using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FastEndpoints;
using FluentResults;

namespace Adoptrix.Application.Commands.Breeds;

public class UpdateBreedCommandHandler(IBreedsRepository breedsRepository)
    : ICommandHandler<UpdateBreedCommand, Result<Breed>>
{
    public async Task<Result<Breed>> ExecuteAsync(UpdateBreedCommand command, CancellationToken cancellationToken)
    {
        // find the breed by id
        var breedResult = await breedsRepository.GetByIdAsync(command.Id, cancellationToken);
        if (breedResult.IsFailed)
        {
            return breedResult.ToResult();
        }

        // update breed properties
        var breed = breedResult.Value;
        breed.Name = command.Name;

        return await breedsRepository.UpdateAsync(breed, cancellationToken);
    }
}