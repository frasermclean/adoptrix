using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services.Repositories;

public interface ISpeciesRepository
{
    Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<Species>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
