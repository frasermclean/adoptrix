using System.Linq.Expressions;
using Adoptrix.Core;
using Adoptrix.Core.Requests;
using Adoptrix.Core.Responses;
using Adoptrix.Logic.Abstractions;
using Adoptrix.Logic.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Persistence.Services;

public class AnimalsRepository(AdoptrixDbContext dbContext) : IAnimalsRepository
{
    public async Task<IEnumerable<AnimalMatch>> SearchAsync(SearchAnimalsRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await dbContext.Animals
            .Where(animal => (request.Name == null || animal.Name.Contains(request.Name)) &&
                             (request.BreedId == null || animal.Breed.Id == request.BreedId) &&
                             (request.SpeciesName == null || animal.Breed.Species.Name == request.SpeciesName) &&
                             (request.Sex == null || animal.Sex == request.Sex))
            .AsNoTracking()
            .OrderBy(animal => animal.Name)
            .Take(request.Limit ?? 10)
            .Select(animal => new AnimalMatch
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Slug = animal.Slug,
                Image = animal.Images.Select(image => image.ToResponse())
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return matches;
    }

    public async Task<TResponse?> GetProjectionAsync<TResponse>(Expression<Func<Animal, bool>> predicate,
        Expression<Func<Animal, TResponse>> selector, CancellationToken cancellationToken = default)
    {
        var response = await dbContext.Animals.Where(predicate)
            .AsNoTracking()
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);

        return response;
    }

    public async Task<Animal?> GetAsync(Guid animalId, CancellationToken cancellationToken)
    {
        var animal = await dbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal;
    }

    public async Task AddAsync(Animal animal, CancellationToken cancellationToken)
    {
        dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
