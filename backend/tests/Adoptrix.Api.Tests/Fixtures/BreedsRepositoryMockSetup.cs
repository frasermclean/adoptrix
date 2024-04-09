using Adoptrix.Application.Models;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain.Models.Factories;

namespace Adoptrix.Api.Tests.Fixtures;

public static class BreedsRepositoryMockSetup
{
    public const string UnknownBreedName = "unknown-breed";

    public static Mock<IBreedsRepository> SetupDefaults(this Mock<IBreedsRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<Guid?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid? _, bool? _, CancellationToken _) => BreedFactory.CreateMany(searchResultsCount)
                .Select(breed => new SearchBreedsResult
                {
                    Id = breed.Id, Name = breed.Name, SpeciesId = breed.Species.Id, AnimalIds = Enumerable.Empty<Guid>()
                }));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? null
                : BreedFactory.Create(breedId));

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName == UnknownBreedName
                ? null
                : BreedFactory.Create(name: breedName));


        return mock;
    }
}
