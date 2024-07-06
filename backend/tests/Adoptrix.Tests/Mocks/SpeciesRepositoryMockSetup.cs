using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Core.Contracts.Requests.Species;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mocks;

public static class SpeciesRepositoryMockSetup
{
    public static readonly Guid UnknownSpeciesId = Guid.NewGuid();
    private const string UnknownSpeciesName = "unknown-species";

    public static Mock<ISpeciesRepository> SetupDefaults(this Mock<ISpeciesRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository => repository.SearchAsync(It.IsAny<SearchSpeciesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.CreateMany(searchResultsCount).Select(species => new SpeciesMatch
            {
                SpeciesId = species.Id,
                SpeciesName = species.Name,
                BreedCount = species.Breeds.Count,
                AnimalCount = Random.Shared.Next(5)
            }));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == UnknownSpeciesId
                ? null
                : SpeciesFactory.Create());

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? null
                : SpeciesFactory.Create());

        return mock;
    }
}
