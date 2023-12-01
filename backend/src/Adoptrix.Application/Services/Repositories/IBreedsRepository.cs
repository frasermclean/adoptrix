using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(Species? species = null, bool withAnimals = false,
        CancellationToken cancellationToken = default);

    Task<Result<Breed>> GetByIdAsync(int breedId, CancellationToken cancellationToken = default);
    Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<Breed>> AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result<Breed>> UpdateAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken = default);
}