using Adoptrix.Application.Models;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services.Repositories;

public interface IAnimalsRepository
{
    Task<IEnumerable<SearchAnimalsResult>> SearchAsync(string? animalName = null, Guid? breedId = null,
        CancellationToken cancellationToken = default);

    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}
