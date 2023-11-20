using Adoptrix.Domain;

namespace Adoptrix.Application.Services.Repositories;

public interface IAnimalsRepository
{
    Task<IEnumerable<Animal>> SearchAsync(string? name = null, Species? species = null,
        CancellationToken cancellationToken = default);

    Task<Animal> AddAsync(Animal animal, CancellationToken cancellationToken = default);
}