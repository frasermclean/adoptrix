using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Logic.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class SpeciesRepository(AdoptrixDbContext dbContext) : ISpeciesRepository
{
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
