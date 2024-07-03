using Adoptrix.Domain.Contracts.Requests;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Models;

namespace Adoptrix.Application.Services;

public interface IAnimalsRepository
{
    Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request, CancellationToken cancellationToken = default);
    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}
