using Adoptrix.Application.Services;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Api.Tests.Fixtures;

public static class SpeciesRepositoryMockSetup
{
    private const string UnknownSpeciesName = "unknown-species";

    public static Mock<ISpeciesRepository> SetupDefaults(this Mock<ISpeciesRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.CreateMany(searchResultsCount));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? null
                : SpeciesFactory.Create());

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? null
                : SpeciesFactory.Create());

        return mock;
    }
}
