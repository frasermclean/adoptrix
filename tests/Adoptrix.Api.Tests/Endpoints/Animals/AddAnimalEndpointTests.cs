using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Contracts.Requests;
using Adoptrix.Contracts.Responses;
using Adoptrix.Core;
using Adoptrix.Logic;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class AddAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    private readonly HttpClient adminClient = fixture.CreateClient(UserRoles.Administrator);
    private readonly HttpClient userClient = fixture.CreateClient();

    [Fact]
    public async Task AddAnimal_WithValidRequest_ShouldReturnCreated()
    {
        // arrange
        var request = CreateRequest("Sasha", "What a great dog", 2, Sex.Female);

        // act
        var (message, response) =
            await adminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
        response.Name.Should().Be("Sasha");
        response.Sex.Should().Be("Female");
        response.DateOfBirth.Should().Be(request.DateOfBirth);
        response.Slug.Should().Be("sasha-2020-01-01");
        response.Age.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddAnimal_WithInvalidBreedId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(breedId: -1);

        // act
        var (message, response) =
            await adminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, ErrorResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.BadRequest);
        response.Errors.Should().ContainSingle().Which.Key.Should().Be("breedId");
    }

    [Fact]
    public async Task AddAnimal_WithInvalidRole_ShouldReturnForbidden()
    {
        // arrange
        var request = CreateRequest();

        // act
        var testResult = await userClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }

    private static AddAnimalRequest CreateRequest(string name = "Buddy", string? description = null, int breedId = 1,
        Sex sex = Sex.Male, DateOnly? dateOfBirth = null, Guid? userId = null) => new()
    {
        Name = name,
        Description = description,
        BreedId = breedId,
        Sex = sex.ToString(),
        DateOfBirth = dateOfBirth ?? new DateOnly(2020, 1, 1),
        UserId = userId ?? Guid.NewGuid()
    };
}
