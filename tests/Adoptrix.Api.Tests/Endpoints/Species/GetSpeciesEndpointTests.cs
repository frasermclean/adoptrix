using System.Net;
using System.Net.Http.Json;
using Adoptrix.Api.Endpoints.Species;
using Adoptrix.Api.Tests.Fixtures;

namespace Adoptrix.Api.Tests.Endpoints.Species;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class GetSpeciesEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task GetSpecies_WithKnownSpeciesName_ShouldReturnOk()
    {
        // arrange
        const string speciesName = "Dog";

        // act
        var message = await fixture.Client.GetAsync($"/api/species/{speciesName}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.OK);
        var response = await message.Content.ReadFromJsonAsync<SpeciesResponse>();
        response.Should().NotBeNull();
        response!.Id.Should().BePositive();
        response.Name.Should().Be(speciesName);
        response.BreedCount.Should().BePositive();
        response.AnimalCount.Should().BePositive();
    }

    [Fact]
    public async Task GetSpecies_WithUnknownSpeciesName_ShouldReturnNotFound()
    {
        // arrange
        const string speciesName = "Unknown";

        // act
        var message = await fixture.Client.GetAsync($"/api/species/{speciesName}");

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
