using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Extensions;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Factories;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public static class AnimalsServiceMock
{
    public static Mock<IAnimalsService> CreateInstance(int searchResultsCount = 3)
    {
        var mock = new Mock<IAnimalsService>();

        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, Guid? _, CancellationToken _) => AnimalFactory.CreateMany(searchResultsCount)
                .Select(animal => new SearchAnimalsResult
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    SpeciesName = animal.Breed.Species.Name,
                    BreedName = animal.Breed.Name,
                    Sex = animal.Sex,
                    DateOfBirth = animal.DateOfBirth,
                    CreatedAt = animal.CreatedAt,
                    Image = animal.Images.Select(image => new AnimalImageResponse
                        {
                            Id = image.Id, Description = image.Description, IsProcessed = image.IsProcessed
                        })
                        .FirstOrDefault(),
                }));

        mock.Setup(repository => repository.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == Guid.Empty
                ? new Result<Animal>().WithError(new AnimalNotFoundError(Guid.Empty))
                : AnimalFactory.Create(animalId));

        mock.Setup(repository =>
                repository.AddAsync(It.IsAny<SetAnimalRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SetAnimalRequest request, CancellationToken _) =>
                Result.Ok(request.ToAnimal(BreedFactory.Create(request.BreedId))));

        mock.Setup(repository =>
                repository.UpdateAsync(It.IsAny<Guid>(), It.IsAny<SetAnimalRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, SetAnimalRequest request, CancellationToken _) => animalId == Guid.Empty
                ? new AnimalNotFoundError(Guid.Empty)
                : Result.Ok(request.ToAnimal(BreedFactory.Create(request.BreedId))));

        mock.Setup(repository => repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == Guid.Empty
                ? new AnimalNotFoundError(Guid.Empty)
                : Result.Ok());

        mock.Setup(service => service.AddImagesAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<AnimalImage>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, IEnumerable<AnimalImage> images, CancellationToken _) => animalId == Guid.Empty
                ? new AnimalNotFoundError(Guid.Empty)
                : Result.Ok(AnimalFactory.Create(animalId, imageCount: images.Count())));

        return mock;
    }
}
