using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchAsync(CancellationToken cancellationToken = default);
    Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<Breed>> AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int breedId, CancellationToken cancellationToken = default);
}