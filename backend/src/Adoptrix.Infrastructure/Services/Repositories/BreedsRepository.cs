using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Infrastructure.Services.Repositories;

public class BreedsRepository(AdoptrixDbContext dbContext)
    : IBreedsRepository
{
    public async Task<IEnumerable<SearchBreedsResult>> SearchBreedsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Breeds
            .Select(breed => new SearchBreedsResult
            {
                Name = breed.Name,
                Species = breed.Species.Name,
                AnimalCount = breed.Animals.Count,
                AnimalIds = breed.Animals.Select(animal => animal.Id)
            })
            .OrderBy(result => result.Name)
            .ToListAsync(cancellationToken);
    }
}