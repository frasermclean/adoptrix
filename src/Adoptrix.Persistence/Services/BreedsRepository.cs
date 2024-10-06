using Adoptrix.Core;
using Adoptrix.Logic.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class BreedsRepository(AdoptrixDbContext dbContext) : IBreedsRepository
{
    public async Task<Breed?> GetAsync(int breedId, CancellationToken cancellationToken = default)
    {
        var breed = await dbContext.Breeds.Where(breed => breed.Id == breedId)
            .Include(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return breed;
    }
}
