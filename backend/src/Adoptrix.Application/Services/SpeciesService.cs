using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface ISpeciesService
{
    Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default);
}

public class SpeciesService(ISpeciesRepository speciesRepository) : ISpeciesService
{
    public Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return speciesRepository.GetAllAsync(cancellationToken);
    }
}
