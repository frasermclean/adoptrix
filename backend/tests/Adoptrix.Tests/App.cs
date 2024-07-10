using System.Net.Http.Headers;
using Adoptrix.Application.Services.Abstractions;
using Adoptrix.Tests.Mocks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Tests;

[DisableWafCache]
public class App : AppFixture<Program>
{
    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; } = new Mock<IAnimalsRepository>().SetupDefaults();
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; } = new Mock<IBreedsRepository>().SetupDefaults();
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; } = new Mock<ISpeciesRepository>().SetupDefaults();
    public Mock<IEventPublisher> EventPublisherMock { get; } = new Mock<IEventPublisher>().SetupDefaults();

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

        // replace repository services with mocks
        services.RemoveAll<IAnimalsRepository>()
            .AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object);
        services.RemoveAll<IBreedsRepository>()
            .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object);
        services.RemoveAll<ISpeciesRepository>()
            .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object);
    }

    protected override Task SetupAsync()
    {
        return Task.CompletedTask;
    }
}
