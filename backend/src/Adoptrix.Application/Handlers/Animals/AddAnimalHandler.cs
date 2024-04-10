using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Handlers.Animals;

public class AddAnimalHandler(IAnimalsRepository animalsRepository, IBreedsRepository breedsRepository)
    : IRequestHandler<AddAnimalRequest, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var breed = await breedsRepository.GetByIdAsync(request.BreedId, cancellationToken);
        if (breed is null)
        {
            return new BreedNotFoundError(request.BreedId);
        }

        var animal = new Animal
        {
            Name = request.Name,
            Description = request.Description,
            Breed = breed,
            Sex = request.Sex,
            DateOfBirth = request.DateOfBirth,
            CreatedBy = request.UserId
        };

        await animalsRepository.AddAsync(animal, cancellationToken);

        return animal;
    }
}
