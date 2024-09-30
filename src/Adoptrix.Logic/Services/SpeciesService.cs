using Adoptrix.Contracts.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Errors;
using Adoptrix.Persistence.Services;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Logic.Services;

public interface ISpeciesService
{
    Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SpeciesResponse>> GetAsync(string speciesName, CancellationToken cancellationToken = default);
}

public class SpeciesService(AdoptrixDbContext dbContext) : ISpeciesService
{
    public async Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var matches = await dbContext.Species
            .AsNoTracking()
            .Select(species => new SpeciesMatch
            {
                Id = species.Id,
                Name = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .Where(match => request.WithAnimals == null || !request.WithAnimals.Value || match.AnimalCount > 0)
            .OrderByDescending(match => match.AnimalCount)
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<Result<SpeciesResponse>> GetAsync(string speciesName,
        CancellationToken cancellationToken = default)
    {
        var response = await dbContext.Species.Where(species => species.Name == speciesName)
            .AsNoTracking()
            .Select(species => new SpeciesResponse
            {
                Id = species.Id,
                Name = species.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        return response is null
            ? new SpeciesNotFoundError(speciesName)
            : response;
    }
}
