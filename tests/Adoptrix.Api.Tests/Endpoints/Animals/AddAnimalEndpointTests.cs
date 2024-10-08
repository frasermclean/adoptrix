﻿using System.Net;
using Adoptrix.Api.Endpoints.Animals;
using Adoptrix.Api.Tests.Fixtures;
using Adoptrix.Core;
using Adoptrix.Core.Responses;

namespace Adoptrix.Api.Tests.Endpoints.Animals;

[Collection(nameof(TestContainersCollection))]
[Trait("Category", "Integration")]
public class AddAnimalEndpointTests(TestContainersFixture fixture) : TestBase<TestContainersFixture>
{
    [Fact]
    public async Task AddAnimal_WithValidRequest_ShouldReturnCreated()
    {
        // arrange
        var request = CreateRequest("Sasha", "What a great dog", 2, Sex.Female);

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        message.Should().HaveStatusCode(HttpStatusCode.Created);
        message.Headers.Location.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Slug.Should().Be("sasha-2020-01-01");
        response.Name.Should().Be("Sasha");
        response.Sex.Should().Be(Sex.Female);
        response.DateOfBirth.Should().Be(request.DateOfBirth);
    }

    [Fact]
    public async Task AddAnimal_WithInvalidBreedId_ShouldReturnBadRequest()
    {
        // arrange
        var request = CreateRequest(breedId: -1);

        // act
        var (message, response) =
            await fixture.AdminClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, ErrorResponse>(request);

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
        var testResult = await fixture.UserClient.POSTAsync<AddAnimalEndpoint, AddAnimalRequest, AnimalResponse>(request);

        // assert
        testResult.Response.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }

    private static AddAnimalRequest CreateRequest(string name = "Buddy", string? description = null, int breedId = 1,
        Sex sex = Sex.Male, DateOnly? dateOfBirth = null, Guid? userId = null) => new()
    {
        Name = name,
        Description = description,
        BreedId = breedId,
        Sex = sex,
        DateOfBirth = dateOfBirth ?? new DateOnly(2020, 1, 1),
        UserId = userId ?? Guid.NewGuid()
    };
}
