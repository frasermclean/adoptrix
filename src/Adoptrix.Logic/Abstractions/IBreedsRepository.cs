using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using FluentResults;

namespace Adoptrix.Logic.Abstractions;

public interface IBreedsRepository
{
    Task<IEnumerable<BreedMatch>> SearchAsync(SearchBreedsRequest request,
        CancellationToken cancellationToken = default);

    Task<Breed?> GetAsync(int breedId, CancellationToken cancellationToken = default);

    Task<TResponse?> GetAsync<TResponse>(int breedId, Expression<Func<Breed, TResponse>> selector,
        CancellationToken cancellationToken = default);

    Task<Result> AddAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Breed breed, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(Breed breed, CancellationToken cancellationToken = default);
}
