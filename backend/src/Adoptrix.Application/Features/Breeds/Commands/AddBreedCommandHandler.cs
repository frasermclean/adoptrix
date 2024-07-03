using Adoptrix.Application.Services;
using Adoptrix.Domain.Contracts.Requests.Breeds;
using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Breeds.Commands;

public class AddBreedCommandHandler(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
    : IRequestHandler<AddBreedRequest, Result<Breed>>
{
    public async Task<Result<Breed>> Handle(AddBreedRequest request, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByIdAsync(request.SpeciesId, cancellationToken);
        if (species is null)
        {
            return new SpeciesNotFoundError(request.SpeciesId);
        }

        var breed = new Breed
        {
            Name = request.Name, Species = species, CreatedBy = request.UserId
        };
        await breedsRepository.AddAsync(breed, cancellationToken);

        return breed;
    }
}
