using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Breeds;

public class AddBreedHandler(IBreedsRepository breedsRepository, ISpeciesRepository speciesRepository)
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
