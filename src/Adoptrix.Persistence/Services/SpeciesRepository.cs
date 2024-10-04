using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class SpeciesRepository(AdoptrixDbContext dbContext) : ISpeciesRepository
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

    public async Task<Species?> GetAsync(string speciesName, CancellationToken cancellationToken = default)
    {
        var species = await dbContext.Species.FirstOrDefaultAsync(species => species.Name == speciesName, cancellationToken);
        return species;
    }

    public async Task<TResponse?> GetAsync<TResponse>(string speciesName, Expression<Func<Species, TResponse>> selector,
        CancellationToken cancellationToken = default)
    {
        var response = await dbContext.Species.Where(species => species.Name == speciesName)
            .AsNoTracking()
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);

        return response;
    }
}
