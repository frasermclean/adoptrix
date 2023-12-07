﻿using System.Net;
using Adoptrix.Api.Contracts.Responses;
using Adoptrix.Api.Endpoints.Animals.AddAnimal;
using Adoptrix.Api.Endpoints.Animals.GetAnimal;
using Adoptrix.Api.Endpoints.Animals.SearchAnimals;
using Adoptrix.Api.Tests.EntityGenerators;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Commands.Animals;
using Adoptrix.Application.Services;
using Adoptrix.Domain;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests.Endpoints;

public class AnimalsTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    : TestClass<ApiTestFixture>(fixture, outputHelper)
{
    private readonly HttpClient httpClient = fixture.Client;
    private readonly ISqidConverter sqidConverter = fixture.Services.GetRequiredService<ISqidConverter>();

    [Fact]
    public async Task SearchAnimals_WithValidRequest_Should_ReturnOk()
    {
        var command = new SearchAnimalsCommand();

        // act
        var (message, responses) =
            await httpClient
                .GETAsync<SearchAnimalsEndpoint, SearchAnimalsCommand, IEnumerable<AnimalResponse>>(command);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.OK);
        responses.Should().HaveCount(3).And.AllSatisfy(ValidateAnimalResponse);
    }

    [Theory]
    [InlineData(3, HttpStatusCode.OK)]
    [InlineData(MockAnimalsRepository.UnknownAnimalId, HttpStatusCode.NotFound)]
    public async Task GetAnimal_WithValidRequest_Should_ExpectedStatusCode(int animalId,
        HttpStatusCode expectedStatusCode)
    {
        // arrange
        var id = sqidConverter.ConvertToSqid(animalId);
        var command = new GetAnimalCommand { Id = id };

        // act
        var (message, response) =
            await httpClient.GETAsync<GetAnimalEndpoint, GetAnimalCommand, AnimalResponse>(command);

        // assert
        message.StatusCode.Should().Be(expectedStatusCode);
        if (expectedStatusCode == HttpStatusCode.OK)
        {
            ValidateAnimalResponse(response);
        }
    }

    [Theory]
    [InlineData("Fido", "A good boy", "dog", "Labrador", HttpStatusCode.Created)]
    [InlineData("Rufus", "Another good boy", "dog", MockBreedsRepository.UnknownBreedName, HttpStatusCode.BadRequest)]
    [InlineData("Max", "", MockSpeciesRepository.UnknownSpeciesName, "Eastern Gray", HttpStatusCode.BadRequest)]
    public async Task AddAnimal_Should_Return_ExpectedResult(string name, string description, string species,
        string breed, HttpStatusCode expectedStatusCode)
    {
        // arrange
        var command = CreateAddAnimalCommand(name, description, species, breed);

        // act
        var (message, response) =
            await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalCommand, AnimalResponse>(command);

        // assert
        message.StatusCode.Should().Be(expectedStatusCode);
        if (expectedStatusCode == HttpStatusCode.Created)
        {
            message.Headers.Should().ContainKey("Location").WhoseValue.Should().Equal($"api/animals/{response.Id}");
            ValidateAnimalResponse(response);
        }
    }

    [Fact]
    public async Task AddAnimal_WithInvalidParameters_Should_ReturnBadRequest()
    {
        // arrange
        var command = CreateAddAnimalCommand();

        // act
        var message = await httpClient.POSTAsync<AddAnimalEndpoint, AddAnimalCommand>(command);

        // assert
        message.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    private static void ValidateAnimalResponse(AnimalResponse response)
    {
        response.Id.Should().NotBeEmpty();
        response.Name.Should().NotBeEmpty();
        response.Species.Should().NotBeEmpty();
        response.Breed.Should().NotBeEmpty();
        response.Sex.Should().NotBeNull();
        response.DateOfBirth.Should().NotBe(default);
    }

    private static AddAnimalCommand CreateAddAnimalCommand(string? name = null, string? description = null,
        string? species = null, string? breed = null, int ageInYears = 2, Sex sex = Sex.Male) => new()
    {
        Name = name!,
        Description = description,
        Species = species!,
        Breed = breed,
        Sex = sex,
        DateOfBirth = DateOnly.FromDateTime(DateTime.Today - TimeSpan.FromDays(365 * ageInYears))
    };
}