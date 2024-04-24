using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsRepository
{
    Task<IEnumerable<SearchAnimalsMatch>> SearchAsync(SearchAnimalsQuery query,
        CancellationToken cancellationToken = default);

    Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<Result<Animal>> GetAsync(string animalSlug, CancellationToken cancellationToken = default);
    Task AddAsync(Animal animal, CancellationToken cancellationToken = default);
    Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default);
}
