using Adoptrix.Application.Errors;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class SpeciesRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), ISpeciesRepository
{
    public async Task<IEnumerable<Species>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Species
            .OrderBy(species => species.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Species>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var species = await DbContext.Species.FirstOrDefaultAsync(species => species.Id == id, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(id);
    }

    public async Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var species = await DbContext.Species.FirstOrDefaultAsync(species => species.Name == name, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(name);
    }
}
