using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;

namespace Adoptrix.Logic.Abstractions;

public interface ISpeciesRepository
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default);

    Task<Species?> GetAsync(string speciesName, CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(string speciesName, Expression<Func<Species, TResponse>> selector,
        CancellationToken cancellationToken = default);
}
