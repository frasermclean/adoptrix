using System.Linq.Expressions;
using Adoptrix.Core;

namespace Adoptrix.Logic.Abstractions;

public interface IAnimalsRepository
{
    Task<TResponse?> GetProjectionAsync<TResponse>(Expression<Func<Animal, bool>> predicate,
        Expression<Func<Animal, TResponse>> selector, CancellationToken cancellationToken = default);

    Task<Animal?> GetAsync(Guid animalId, CancellationToken cancellationToken);
    Task AddAsync(Animal animal, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
