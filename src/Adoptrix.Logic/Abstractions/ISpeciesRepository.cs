using System.Linq.Expressions;
using Adoptrix.Core;

namespace Adoptrix.Logic.Abstractions;

public interface ISpeciesRepository
{
    Task<Species?> GetAsync(string speciesName, CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(string speciesName, Expression<Func<Species, TResponse>> selector,
        CancellationToken cancellationToken = default);
}
