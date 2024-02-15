using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface ISpeciesService
{
    Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public class SpeciesService(ISpeciesRepository repository) : ISpeciesService
{
    public async Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await repository.GetByNameAsync(name, cancellationToken);
    }
}
