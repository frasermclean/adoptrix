using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

public static class AnimalsRepositoryMockSetup
{
    public static readonly Guid UnknownAnimalId = Guid.NewGuid();

    public static Mock<IAnimalsRepository> SetupDefaults(this Mock<IAnimalsRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<SearchAnimalsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SearchAnimalsQuery _, CancellationToken _) => AnimalFactory.CreateMany(searchResultsCount)
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
                }));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == UnknownAnimalId
                ? null
                : AnimalFactory.Create(animalId));

        mock.Setup(repository => repository.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => AnimalFactory.Create(animalId));

        mock.Setup(repository => repository.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string animalSlug, CancellationToken _) => AnimalFactory.Create(slug: animalSlug));

        return mock;
    }
}
