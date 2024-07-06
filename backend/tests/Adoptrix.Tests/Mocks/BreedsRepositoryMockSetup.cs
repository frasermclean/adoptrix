using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Domain.Contracts.Responses;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mocks;

public static class BreedsRepositoryMockSetup
{
    public static Mock<IBreedsRepository> SetupDefaults(this Mock<IBreedsRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<Guid?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid? _, bool? _, CancellationToken _) => BreedFactory.CreateMany(searchResultsCount)
                .Select(breed => new BreedMatch
                {
                    Id = breed.Id, Name = breed.Name, SpeciesId = breed.Species.Id, AnimalCount = 0,
                }));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => BreedFactory.Create(breedId));

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => BreedFactory.Create(name: breedName));

        return mock;
    }
}
