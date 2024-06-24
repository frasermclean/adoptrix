using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;

namespace Adoptrix.Application.Services;

public interface IAnimalsRepository
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsQuery query,
        CancellationToken cancellationToken = default);

    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}
