using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class SpeciesRepository(AdoptrixDbContext dbContext)
    : ISpeciesRepository
{
    public Task<Result<Species>> GetSpeciesByIdAsync(int speciesId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Species>> GetSpeciesByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var species = await dbContext.Species.FirstOrDefaultAsync(species => species.Name == name, cancellationToken);

        return species is not null
            ? species
            : new SpeciesNotFoundError(name);
    }
}