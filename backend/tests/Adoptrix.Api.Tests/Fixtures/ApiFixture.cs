using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Contracts.Requests;
using Adoptrix.Application.Errors;
using Adoptrix.Application.Extensions;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Models.Factories;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Api.Tests.Fixtures;

public class ApiFixture : WebApplicationFactory<Program>
{
    public Mock<IAnimalsService> AnimalsService { get; } = AnimalsServiceMock.CreateInstance();
    public Mock<IBreedsService> BreedsService { get; } = new();
    public Mock<ISpeciesRepository> SpeciesRepository { get; } = new();
    public Mock<IAnimalImageManager> AnimalImageManager { get; } = new();

    public const int SearchResultsCount = 3;
    public const string UnknownBreedName = "unknown-breed";
    public const string UnknownSpeciesName = "unknown-species";

    public ApiFixture()
    {
        SetupBreedsServiceMock(BreedsService);
        SetupSpeciesRepositoryMock(SpeciesRepository);
        SetupAnimalImageManagerMock(AnimalImageManager);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(
                    "APPLICATIONINSIGHTS_CONNECTION_STRING",
                    "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://xxxx.applicationinsights.azure.com/"
                )
            });
        });

        builder.ConfigureServices(services =>
        {
            // remove infrastructure services and replace them with mocks
            services.RemoveAll<IAnimalsService>()
                .AddScoped<IAnimalsService>(_ => AnimalsService.Object);
            services.RemoveAll<IBreedsService>()
                .AddScoped<IBreedsService>(_ => BreedsService.Object);
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

    private static void SetupBreedsServiceMock(Mock<IBreedsService> mock)
    {
        mock.Setup(repository =>
                repository.SearchAsync(It.IsAny<Guid?>(), It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid? _, bool? _, CancellationToken _) => BreedFactory.CreateMany(SearchResultsCount)
                .Select(breed => new SearchBreedsResult
                {
                    Id = breed.Id, Name = breed.Name, SpeciesId = breed.Species.Id, AnimalIds = Enumerable.Empty<Guid>()
                }));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? new BreedNotFoundError(Guid.Empty)
                : BreedFactory.Create(breedId));

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string breedName, CancellationToken _) => breedName == UnknownBreedName
                ? new BreedNotFoundError(UnknownBreedName)
                : BreedFactory.Create(name: breedName));

        mock.Setup(repository => repository.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid breedId, CancellationToken _) => breedId == Guid.Empty
                ? new BreedNotFoundError(Guid.Empty)
                : Result.Ok());
    }

    private static void SetupSpeciesRepositoryMock(Mock<ISpeciesRepository> mock)
    {
        mock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.CreateMany(SearchResultsCount));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? new SpeciesNotFoundError(Guid.Empty)
                : SpeciesFactory.Create());

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? new SpeciesNotFoundError(speciesName)
                : SpeciesFactory.Create());
    }

    private static void SetupAnimalImageManagerMock(Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);
    }
}
