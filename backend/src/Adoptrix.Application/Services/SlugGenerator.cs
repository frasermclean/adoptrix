using System.Text;
using Adoptrix.Domain.Models;
using FluentResults;
using Humanizer;

namespace Adoptrix.Application.Services;

public interface ISlugGenerator
{
    Task<Result<string>> GenerateAsync(string name, Breed breed, DateOnly dateOfBirth,
        CancellationToken cancellationToken = default);
}

public class SlugGenerator(IAnimalsRepository animalsRepository) : ISlugGenerator
{
    public async Task<Result<string>> GenerateAsync(string name, Breed breed, DateOnly dateOfBirth,
        CancellationToken cancellationToken = default)
    {
        var validationResult = ValidateNames(name, breed.Name);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var slug = new StringBuilder()
            .Append(name.Kebaberize())
            .Append('-')
            .Append(breed.Name.Kebaberize())
            .Append('-')
            .Append($"{dateOfBirth.Year}-{dateOfBirth.Month:D2}-{dateOfBirth.Day:D2}")
            .ToString();

        // ensure slug is unique
        var animal = await animalsRepository.GetBySlugAsync(slug, cancellationToken);
        return animal is null
            ? Result.Ok(slug)
            : Result.Fail("Generated slug is not unique");
    }

    private static Result ValidateNames(string animalName, string breedName)
    {
        var animalResult = Result.FailIf(animalName.Length > Animal.NameMaxLength, "Animal name is too long");
        var breedResult = Result.FailIf(breedName.Length > Breed.NameMaxLength, "Breed name is too long");

        return Result.Merge(animalResult, breedResult);
    }
}
