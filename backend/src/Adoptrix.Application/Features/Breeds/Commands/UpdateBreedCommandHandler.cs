using Adoptrix.Application.Services;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public class UpdateBreedCommandHandler(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : IRequestHandler<UpdateBreedRequest, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(UpdateBreedRequest request, CancellationToken cancellationToken = default)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesId);
        }

        breed.Name = request.Name;
        breed.Species = species;
        await breedsRepository.UpdateAsync(breed, cancellationToken);

        return breed;
    }
}
