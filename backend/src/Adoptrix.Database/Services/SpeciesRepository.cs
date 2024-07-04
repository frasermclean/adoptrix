using Adoptrix.Application.Services;
using Adoptrix.Domain.Contracts.Requests.Species;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class SpeciesRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), ISpeciesRepository
{
    public async Task<IEnumerable<SpeciesMatch>> SearchAsync(SearchSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Species
            .Select(species => new SpeciesMatch
            {
                SpeciesId = species.Id,
                SpeciesName = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = species.Breeds.Count(breed => breed.Animals.Count > 0)
            })
            .Where(match => !request.WithAnimals || match.AnimalCount > 0)
            .OrderByDescending(match => match.AnimalCount)
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
