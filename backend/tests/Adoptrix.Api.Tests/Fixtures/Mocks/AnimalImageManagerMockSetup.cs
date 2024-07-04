using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using FluentResults;

namespace Adoptrix.Api.Tests.Fixtures.Mocks;

public static class AnimalImageManagerMockSetup
{
    public static Mock<IAnimalImageManager> SetupDefaults(this Mock<IAnimalImageManager> mock)
    {
        return mock;
    }

}
