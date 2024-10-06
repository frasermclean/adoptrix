using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Errors;
using FluentResults;

namespace Adoptrix.Logic.Services;

public interface ISpeciesService
{
    Task<Result<SpeciesResponse>> GetAsync(string speciesName, CancellationToken cancellationToken = default);
}

public class SpeciesService(ISpeciesRepository speciesRepository) : ISpeciesService
{
    public async Task<Result<SpeciesResponse>> GetAsync(string speciesName,
        CancellationToken cancellationToken = default)
    {
        var response = await speciesRepository.GetAsync(speciesName, species => new SpeciesResponse
        {
            Id = species.Id,
            Name = species.Name
        }, cancellationToken);

        return response is null
            ? new SpeciesNotFoundError(speciesName)
            : response;
    }
}
