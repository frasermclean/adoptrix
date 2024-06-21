using Adoptrix.Application.Services;
using Adoptrix.Domain.Commands.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public class AddBreedCommandHandler(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : IRequestHandler<AddBreedCommand, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(AddBreedCommand command, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByIdAsync(command.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(command.SpeciesId);
        }

        var breed = new Breed
        {
            Name = command.Name, Species = species, CreatedBy = command.UserId
        };
        await breedsRepository.AddAsync(breed, cancellationToken);

        return breed;
    }
}
