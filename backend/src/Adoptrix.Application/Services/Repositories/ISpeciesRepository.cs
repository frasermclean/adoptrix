using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services.Repositories;

public interface ISpeciesRepository
{
    Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
