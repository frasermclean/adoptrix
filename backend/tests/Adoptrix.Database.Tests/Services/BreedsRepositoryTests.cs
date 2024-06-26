﻿using Adoptrix.Application.Services;
using Adoptrix.Database.Tests.Fixtures;
using Adoptrix.Domain.Models;
using Adoptrix.Tests.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace Adoptrix.Database.Tests.Services;

[Trait("Category", "Integration")]
[Collection(nameof(DatabaseCollection))]
public class BreedsRepositoryTests
{
    private readonly IBreedsRepository breedsRepository;
    private readonly ISpeciesRepository speciesRepository;

    public BreedsRepositoryTests(DatabaseFixture fixture)
    {
        var collection = fixture.GetRepositoryCollection();
        (_, breedsRepository, speciesRepository) = collection;
    }

    [Fact]
    public async Task SearchAsync_WithNoParameters_ShouldReturnAllBreeds()
    {
        // act
        var results = await breedsRepository.SearchAsync();

        // assert
        results.Should().HaveCountGreaterOrEqualTo(3);
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByIdAsync_WithKnownId_ShouldReturnBreed(Guid breedId, string expectedName)
    {
        // act
        var breed = await breedsRepository.GetByIdAsync(breedId);

        // assert
        breed.Should().BeOfType<Breed>().Which.Name.Should().Be(expectedName);
    }

    [Fact]
    public async Task GetByIdAsync_WithUnknownId_ShouldReturnNull()
    {
        // arrange
        var unknownId = Guid.Empty;

        // act
        var breed = await breedsRepository.GetByIdAsync(unknownId);

        // assert
        breed.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GetKnownBreeds))]
    public async Task GetByNameAsync_WithKnownName_ShouldReturnBreed(Guid expectedId, string breedName)
    {
        // act
        var breed = await breedsRepository.GetByNameAsync(breedName);

        // assert
        breed.Should().BeOfType<Breed>().Which.Id.Should().Be(expectedId);
    }

    [Fact]
    public async Task AddAsync_WithValidBreed_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breedToAdd = BreedFactory.Create(name: "Poodle", species: species);

        // act
        await breedsRepository.AddAsync(breedToAdd);
        var breed = await breedsRepository.GetByIdAsync(breedToAdd.Id);

        // assert
        breed.Should().BeOfType<Breed>().Which.Name.Should().Be(breedToAdd.Name);
    }

    [Fact]
    public async Task UpdateAsync_WithValidBreed_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breedToAdd = BreedFactory.Create(name: "Border-Collie", species: species);
        await breedsRepository.AddAsync(breedToAdd);

        // act
        breedToAdd.Name = "Border Collie";
        await breedsRepository.UpdateAsync(breedToAdd);
        var breed = await breedsRepository.GetByIdAsync(breedToAdd.Id);

        // assert
        breed.Should().BeOfType<Breed>().Which.Name.Should().Be("Border Collie");
    }

    [Fact]
    public async Task DeleteAsync_WithKnownId_ShouldPass()
    {
        // arrange
        var species = await speciesRepository.GetByIdAsync(SpeciesIds.Dog);
        var breedToDelete = BreedFactory.Create(name: "Pug", species: species);
        await breedsRepository.AddAsync(breedToDelete);

        // act
        await breedsRepository.DeleteAsync(breedToDelete);
        var breed = await breedsRepository.GetByIdAsync(breedToDelete.Id);

        // assert
        breed.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithUnknownId_ShouldThrowException()
    {
        // arrange
        var unknownBreed = BreedFactory.Create();

        // act
        var act = async () => await breedsRepository.DeleteAsync(unknownBreed);

        // assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    public static TheoryData<Guid, string> GetKnownBreeds() => new()
    {
        { BreedIds.LabradorRetriever, "Labrador Retriever" },
        { BreedIds.GermanShepherd, "German Shepherd" },
        { BreedIds.GoldenRetriever, "Golden Retriever" }
    };
}
