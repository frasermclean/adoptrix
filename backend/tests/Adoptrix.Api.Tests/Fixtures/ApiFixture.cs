﻿using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Fixtures.Mocks;
using Adoptrix.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Adoptrix.Api.Tests.Fixtures;

public class ApiFixture : WebApplicationFactory<Program>
{
    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; }
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; }
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; }
    public Mock<IAnimalImageManager> AnimalImageManager { get; }
    public Mock<IEventPublisher> EventPublisherMock { get; }

    public const int SearchResultsCount = 3;

    public ApiFixture()
    {
        AnimalsRepositoryMock = new Mock<IAnimalsRepository>().SetupDefaults();
        BreedsRepositoryMock = new Mock<IBreedsRepository>().SetupDefaults();
        SpeciesRepositoryMock = new Mock<ISpeciesRepository>().SetupDefaults();
        AnimalImageManager = new Mock<IAnimalImageManager>().SetupDefaults();
        EventPublisherMock = new Mock<IEventPublisher>().SetupDefaults();
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

        builder.ConfigureLogging(loggingBuilder => { loggingBuilder.ClearProviders(); });

        builder.ConfigureServices(services =>
        {
            // replace database services with mocks
            services.RemoveAll<IAnimalsRepository>()
                .AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object);
            services.RemoveAll<IBreedsRepository>()
                .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object);
            services.RemoveAll<ISpeciesRepository>()
                .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object);

            // replace storage services with mocks
            services.RemoveAll<IAnimalImageManager>()
                .AddScoped<IAnimalImageManager>(_ => AnimalImageManager.Object);
            services.RemoveAll<IEventPublisher>()
                .AddSingleton<IEventPublisher>(_ => EventPublisherMock.Object);

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
}
