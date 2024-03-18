using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Generators;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Api.Tests.Fixtures;

public class ApiFixture : WebApplicationFactory<Program>
{
    public Mock<IAnimalsRepository> AnimalsRepository { get; } = new();
    public Mock<IBreedsRepository> BreedsRepository { get; } = new();
    public Mock<ISpeciesRepository> SpeciesRepository { get; } = new();
    public Mock<IAnimalImageManager> AnimalImageManager { get; } = new();

    public const int SearchResultsCount = 3;
    public const string UnknownBreedName = "unknown-breed";
    public const string UnknownSpeciesName = "unknown-species";

    public ApiFixture()
    {
        SetupAnimalRepositoryMock(AnimalsRepository);
        SetupBreedsRepositoryMock(BreedsRepository);
        SetupSpeciesRepositoryMock(SpeciesRepository);
        SetupAnimalImageManagerMock(AnimalImageManager);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove infrastructure services and replace them with mocks
            services.RemoveAll<IAnimalsRepository>()
                .AddScoped<IAnimalsRepository>(_ => AnimalsRepository.Object);
            services.RemoveAll<IBreedsRepository>()
                .AddScoped<IBreedsRepository>(_ => BreedsRepository.Object);
            services.RemoveAll<ISpeciesRepository>()
                .AddScoped<ISpeciesRepository>(_ => SpeciesRepository.Object);
            services.RemoveAll<IAnimalImageManager>()
                .AddScoped<IAnimalImageManager>(_ => AnimalImageManager.Object);

            // remove the real event publisher and replace it with a mock
            services.RemoveAll<IEventPublisher>();
            services.AddScoped<IEventPublisher, MockEventPublisher>();

            // add test auth handler
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        // configure the client to use the test auth scheme
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.SchemeName);
    }

    private static void SetupAnimalRepositoryMock(Mock<IAnimalsRepository> mock,
        int searchResultsCount = SearchResultsCount)
    {
        mock.Setup(repository =>
                repository.SearchAnimalsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, string _, CancellationToken _) => AnimalGenerator.Generate(searchResultsCount)
                .Select(animal => new SearchAnimalsResult
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Description = animal.Description,
                    SpeciesId = animal.Species.Id,
                    BreedId = animal.Breed.Id,
                    Sex = animal.Sex,
                    DateOfBirth = animal.DateOfBirth,
                    CreatedAt = animal.CreatedAt,
                    Images = animal.Images
                }));

        mock.Setup(repository => repository.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == Guid.Empty
                ? new Result<Animal>().WithError(new AnimalNotFoundError(Guid.Empty))
                : AnimalGenerator.Generate(animalId));

        mock.Setup(repository => repository.AddAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Animal animal, CancellationToken _) => Result.Ok(animal));

        mock.Setup(repository => repository.UpdateAsync(It.IsAny<Animal>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Animal animal, CancellationToken _) => animal.Id == Guid.Empty
                ? new AnimalNotFoundError(Guid.Empty)
                : Result.Ok());

        mock.Setup(repository => repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid animalId, CancellationToken _) => animalId == Guid.Empty
                ? new AnimalNotFoundError(Guid.Empty)
                : Result.Ok());
    }

    private static void SetupBreedsRepositoryMock(Mock<IBreedsRepository> mock)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<Species>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string _, bool _, CancellationToken _) => AnimalGenerator.Generate(SearchResultsCount)
                .Select(animal => new SearchBreedsResult
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    SpeciesId = animal.Species.Id,
                    AnimalIds = Enumerable.Empty<Guid>()
                }));
        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? new BreedNotFoundError(Guid.Empty)
                : BreedGenerator.Generate(breedId));

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName == UnknownBreedName
                ? new BreedNotFoundError(UnknownBreedName)
                : BreedGenerator.Generate(breedName: breedName));

        mock.Setup(repository => repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? new BreedNotFoundError(Guid.Empty)
                : Result.Ok());
    }

    private static void SetupSpeciesRepositoryMock(Mock<ISpeciesRepository> mock)
    {
        mock.Setup(repository => repository.SearchSpeciesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesGenerator.Generate(SearchResultsCount));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? new SpeciesNotFoundError(Guid.Empty)
                : SpeciesGenerator.Generate());

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? new SpeciesNotFoundError(speciesName)
                : SpeciesGenerator.Generate());
    }

    private static void SetupAnimalImageManagerMock(Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);
    }
}
