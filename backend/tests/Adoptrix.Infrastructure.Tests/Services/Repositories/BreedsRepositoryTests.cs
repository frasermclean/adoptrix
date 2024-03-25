using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Storage.Tests.Fixtures;

namespace Adoptrix.Infrastructure.Storage.Tests.Services.Repositories;

[Trait("Category", "Integration")]
public class BreedsRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly IBreedsRepository repository = fixture.BreedsRepository!;

    [Fact]
    public async Task SearchAsync_With_NoParameters_Returns_AllBreeds()
    {
        // act
        var results = await repository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }
}
