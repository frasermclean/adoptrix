using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class SpeciesRepository(AdoptrixDbContext dbContext)
    : ISpeciesRepository
{
    public async Task<IEnumerable<Species>> GetAllSpeciesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Species.ToListAsync(cancellationToken);
    }

    public async Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var species = await dbContext.Species.FirstOrDefaultAsync(species => species.Name == name, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(name);
    }
}