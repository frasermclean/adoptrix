using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Tests.Shared.Factories;

namespace Adoptrix.Tests.Mocks;

public static class AnimalsRepositoryMockSetup
{
    public static readonly Guid UnknownAnimalId = Guid.NewGuid();

    public static Mock<IAnimalsRepository> SetupDefaults(this Mock<IAnimalsRepository> mock, int searchResultsCount = 3)
    {
        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == UnknownAnimalId
                ? null
                : AnimalFactory.Create(animalId));

        return mock;
    }
}
