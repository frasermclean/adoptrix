using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface ISpeciesService
{
    Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<Species>> GetByIdAsync(Guid speciesId, CancellationToken cancellationToken = default);
    Task<Result<Species>> GetByNameAsync(string speciesName, CancellationToken cancellationToken = default);
}

public class SpeciesService(ISpeciesRepository speciesRepository) : ISpeciesService
{
    public Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return speciesRepository.GetAllAsync(cancellationToken);
    }
    public async Task<Result<Species>> GetByIdAsync(Guid speciesId, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByIdAsync(speciesId, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(speciesId);
    }

    public async Task<Result<Species>> GetByNameAsync(string speciesName, CancellationToken cancellationToken = default)
    {
        var species = await speciesRepository.GetByNameAsync(speciesName, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(speciesName);
    }
}
