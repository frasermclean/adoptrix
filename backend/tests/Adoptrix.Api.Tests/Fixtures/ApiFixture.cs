using System.Net.Http.Headers;
using Adoptrix.Application.Models;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
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
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; }
    public Mock<IAnimalImageManager> AnimalImageManager { get; } = new();

    public const int SearchResultsCount = 3;

    public ApiFixture()
    {
        AnimalsRepositoryMock = new Mock<IAnimalsRepository>().SetupDefaults();
        BreedsRepositoryMock = new Mock<IBreedsRepository>().SetupDefaults();
        SpeciesRepositoryMock = new Mock<ISpeciesRepository>().SetupDefaults();
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
            // repository mocks
            services.RemoveAll<IAnimalsRepository>()
                .AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object);
            services.RemoveAll<IBreedsRepository>()
                .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object);
            services.RemoveAll<ISpeciesRepository>()
                .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object);
            services.RemoveAll<IAnimalImageManager>()
                .AddScoped<IAnimalImageManager>(_ => AnimalImageManager.Object);

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

    private static void SetupAnimalImageManagerMock(Mock<IAnimalImageManager> mock)
    {
        mock.Setup(manager => manager.UploadImageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Stream>(),
                It.IsAny<string>(), It.IsAny<ImageCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok);
    }
}
