using Adoptrix.Core;
using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;

namespace Adoptrix.Application.Services.Abstractions;

public interface IAnimalsRepository
{
    Task<IReadOnlyList<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken = default);
    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}
