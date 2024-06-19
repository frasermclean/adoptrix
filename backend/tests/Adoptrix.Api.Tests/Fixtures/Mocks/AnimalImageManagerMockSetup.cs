using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

public static class AnimalImageManagerMockSetup
{
    public static Mock<IAnimalImageManager> SetupDefaults(this Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<AnimalImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);

        return mock;
    }

}
