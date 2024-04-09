using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
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
    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; }
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; }
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; } = new();
    public Mock<IAnimalImageManager> AnimalImageManager { get; } = new();

    public const int SearchResultsCount = 3;

    public const string UnknownSpeciesName = "unknown-species";

    public ApiFixture()
    {
        AnimalsRepositoryMock = new Mock<IAnimalsRepository>().SetupDefaults();
        BreedsRepositoryMock = new Mock<IBreedsRepository>().SetupDefaults();
        SetupSpeciesRepositoryMock(SpeciesRepositoryMock);
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
            services.RemoveAll<IAnimalsRepository>()
                .AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object);
            services.RemoveAll<IBreedsRepository>()
                .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object);
            services.RemoveAll<ISpeciesRepository>()
                .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object);
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

    private static void SetupSpeciesRepositoryMock(Mock<ISpeciesRepository> mock)
    {
        mock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(SpeciesFactory.CreateMany(SearchResultsCount));

        mock.Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid speciesId, CancellationToken _) => speciesId == Guid.Empty
                ? null
                : SpeciesFactory.Create());

        mock.Setup(repository => repository.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string speciesName, CancellationToken _) => speciesName == UnknownSpeciesName
                ? null
                : SpeciesFactory.Create());
    }

    private static void SetupAnimalImageManagerMock(Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);
    }
}
