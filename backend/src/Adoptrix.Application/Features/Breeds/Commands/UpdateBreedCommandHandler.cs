using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public class UpdateBreedCommandHandler(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : IRequestHandler<UpdateBreedCommand, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(UpdateBreedCommand command, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(command.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(command.BreedId);
        }

        var species = await speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(command.SpeciesId);
        }

        breed.Name = command.Name;
        breed.Species = species;
        await breedsRepository.UpdateAsync(breed, cancellationToken);

        return breed;
    }
}
