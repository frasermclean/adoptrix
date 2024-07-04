using Adoptrix.Application.Services;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

public static class AnimalImageManagerMockSetup
{
    public static Mock<IAnimalImageManager> SetupDefaults(this Mock<IAnimalImageManager> mock)
    {
        return mock;
    }

}
