using System.Net.Http.Headers;
using Adoptrix.Api.Services;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Api.Tests;

[DisableWafCache]
public class ApiFixture : AppFixture<Program>
{
    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; } = new();
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; } = new();
    public Mock<ISpeciesRepository> SpeciesRepositoryMock { get; } = new();
    public Mock<IEventPublisher> EventPublisherMock { get; } = new();
    public Mock<IBlobContainerManager> AnimalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IBlobContainerManager> OriginalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IUsersService> UsersServiceMock { get; } = new();

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

        // remove selected services
        services.RemoveAll<IAnimalsRepository>()
            .RemoveAll<IBreedsRepository>()
            .RemoveAll<ISpeciesRepository>()
            .RemoveAll<IEventPublisher>()
            .RemoveAll<IBlobContainerManager>()
            .RemoveAll<IUsersService>();

        // replace with mocked services
        services.AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object)
            .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object)
            .AddScoped<ISpeciesRepository>(_ => SpeciesRepositoryMock.Object)
            .AddScoped<IEventPublisher>(_ => EventPublisherMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages,
                (_, _) => AnimalImagesBlobContainerManagerMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages,
                (_, _) => OriginalImagesBlobContainerManagerMock.Object)
            .AddScoped<IUsersService>(_ => UsersServiceMock.Object);
    }
}
