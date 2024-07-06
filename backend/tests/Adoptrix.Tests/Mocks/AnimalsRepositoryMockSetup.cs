using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Contracts.Requests.Animals;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mocks;

public static class AnimalsRepositoryMockSetup
{
    public static readonly Guid UnknownAnimalId = Guid.NewGuid();

    public static Mock<IAnimalsRepository> SetupDefaults(this Mock<IAnimalsRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchAnimalsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SearchAnimalsRequest _, CancellationToken _) => AnimalFactory.CreateMany(searchResultsCount)
                .Select(animal => new AnimalMatch
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

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == UnknownAnimalId
                ? null
                : AnimalFactory.Create(animalId));

        return mock;
    }
}
