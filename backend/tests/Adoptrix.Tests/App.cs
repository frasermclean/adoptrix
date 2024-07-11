﻿using System.Net.Http.Headers;
using Adoptrix.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Tests;

[DisableWafCache]
public class App : AppFixture<Program>
{
    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; } = new();
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; } = new();
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; } = new();
    public Mock<IEventPublisher> EventPublisherMock { get; } = new();

    /// <summary>
    /// HTTP client pre-configured with basic test authentication.
    /// </summary>
    public HttpClient BasicAuthClient => CreateClient(options =>
        options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BasicAuthHandler.SchemeName));

    protected override void ConfigureServices(IServiceCollection services)
    {
        // add test auth handler
        services.AddAuthentication(BasicAuthHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>(BasicAuthHandler.SchemeName, _ => { });

        // replace services with mocks
        services.RemoveAll<IAnimalsRepository>()
            .AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object);
        services.RemoveAll<IBreedsRepository>()
            .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object);
        services.RemoveAll<ISpeciesRepository>()
            .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object);
        services.RemoveAll<IEventPublisher>()
            .AddScoped<IEventPublisher>(_ => EventPublisherMock.Object);
    }

    protected override Task SetupAsync()
    {
        return Task.CompletedTask;
    }
}
