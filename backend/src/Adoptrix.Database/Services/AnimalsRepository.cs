using Adoptrix.Application.Errors;
using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Services;

public class AnimalsRepository(AdoptrixDbContext dbContext, IBatchManager batchManager)
    : Repository(dbContext, batchManager), IAnimalsRepository
{
    private const int SearchLimit = 10;

    public async Task<IEnumerable<SearchAnimalsMatch>> SearchAsync(SearchAnimalsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals
            .AsNoTracking()
            .Where(animal => (query.Name == null || animal.Name.Contains(query.Name)) &&
                             (query.BreedId == null || animal.Breed.Id == query.BreedId) &&
                             (query.SpeciesId == null || animal.Breed.Species.Id == query.SpeciesId) &&
                             (query.Sex == null || animal.Sex == query.Sex))
            .Take(query.Limit ?? SearchLimit)
            .Select(animal => new SearchAnimalsMatch
            {
                Id = animal.Id,
                Name = animal.Name,
                SpeciesName = animal.Breed.Species.Name,
                BreedName = animal.Breed.Name,
                Sex = animal.Sex,
                Slug = animal.Slug,
                DateOfBirth = animal.DateOfBirth,
                CreatedAt = animal.CreatedAt,
                Image = animal.Images.Select(image => new AnimalImageResponse
                    {
                        Id = image.Id, Description = image.Description, IsProcessed = image.IsProcessed
                    })
                    .FirstOrDefault(),
            })
            .OrderBy(animal => animal.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Animal?> GetByIdAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Animal?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.Animals.Where(animal => animal.Slug == slug)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Result<Animal>> GetAsync(Guid animalId, CancellationToken cancellationToken = default)
    {
        var animal =  await DbContext.Animals.Where(animal => animal.Id == animalId)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalId)
            : animal;
    }

    public async Task<Result<Animal>> GetAsync(string animalSlug, CancellationToken cancellationToken = default)
    {
        var animal =  await DbContext.Animals.Where(animal => animal.Slug == animalSlug)
            .Include(animal => animal.Breed)
            .ThenInclude(breed => breed.Species)
            .FirstOrDefaultAsync(cancellationToken);

        return animal is null
            ? new AnimalNotFoundError(animalSlug)
            : animal;
    }

    public async Task AddAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        DbContext.Animals.Add(animal);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Animal animal, CancellationToken cancellationToken = default)
    {
        DbContext.Animals.Remove(animal);
        await SaveChangesAsync(cancellationToken);
    }
}
