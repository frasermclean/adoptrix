using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IAnimalsService
{
    Task<Result<Animal>> AddAnimalAsync(string name, string? description, string speciesName, string? breedName,
        Sex? sex, DateOnly dateOfBirth, Guid createdBy, CancellationToken cancellationToken = default);
}

public class AnimalsService(IAnimalsRepository repository, ISpeciesService speciesService, IBreedsService breedsService)
    : IAnimalsService
{
    public async Task<Result<Animal>> AddAnimalAsync(string name, string? description, string speciesName,
        string? breedName, Sex? sex, DateOnly dateOfBirth, Guid createdBy, CancellationToken cancellationToken)
    {
        // find species
        var speciesResult = await speciesService.GetByNameAsync(speciesName, cancellationToken);
        if (speciesResult.IsFailed)
        {
            return speciesResult.ToResult();
        }

        // find breed if breed name was specified
        var breedResult = breedName is not null
            ? await breedsService.GetByNameAsync(breedName, cancellationToken)
            : null;
        if (breedResult?.IsFailed ?? false)
        {
            return breedResult.ToResult();
        }

        var animal = new Animal
        {
            Name = name,
            Description = description,
            Species = speciesResult.Value,
            Breed = breedResult?.Value,
            Sex = sex,
            DateOfBirth = dateOfBirth,
            CreatedBy = createdBy
        };

        return await repository.AddAsync(animal, cancellationToken);
    }
}
