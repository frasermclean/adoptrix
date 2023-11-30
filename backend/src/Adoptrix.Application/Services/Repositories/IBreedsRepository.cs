using Adoptrix.Application.Models;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchBreedsAsync(CancellationToken cancellationToken = default);
    Task<Result<Breed>> GetBreedByNameAsync(string name, CancellationToken cancellationToken = default);
}