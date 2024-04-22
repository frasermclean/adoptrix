using Adoptrix.Application.Features.Species.Queries;
using Adoptrix.Application.Features.Species.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class SpeciesRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), ISpeciesRepository
{
    public async Task<IEnumerable<SearchSpeciesMatch>> SearchAsync(SearchSpeciesQuery query,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Species
            .Select(species => new SearchSpeciesMatch
            {
                SpeciesId = species.Id,
                SpeciesName = species.Name,
                BreedCount = species.Breeds.Count
            })
            .OrderBy(match => match.SpeciesName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Species?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Species.FirstOrDefaultAsync(species => species.Id == id, cancellationToken);
    }

    public async Task<Species?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.Species.FirstOrDefaultAsync(species => species.Name == name, cancellationToken);
    }
}
