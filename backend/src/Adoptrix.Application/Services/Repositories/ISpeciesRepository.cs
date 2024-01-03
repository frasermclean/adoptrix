using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface ISpeciesRepository
{
    Task<IEnumerable<Species>> SearchSpeciesAsync(CancellationToken cancellationToken = default);
    Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}