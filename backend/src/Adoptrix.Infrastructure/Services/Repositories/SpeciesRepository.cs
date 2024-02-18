using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class SpeciesRepository(AdoptrixDbContext dbContext) : Repository(dbContext), ISpeciesRepository
{
    public async Task<IEnumerable<Species>> SearchSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Species.ToListAsync(cancellationToken);
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
