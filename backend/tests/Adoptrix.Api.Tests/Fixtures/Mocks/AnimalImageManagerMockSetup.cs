using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using FluentResults;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

public static class AnimalImageManagerMockSetup
{
    public static Mock<IAnimalImageManager> SetupDefaults(this Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);

        return mock;
    }

}
