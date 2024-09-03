using System.Net.Http.Headers;
using Adoptrix.Logic;
using Adoptrix.Logic.Services;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Api.Tests.Fixtures;

[DisableWafCache]
public class MockServicesFixture : AppFixture<Program>
{
    public Mock<IEventPublisher> EventPublisherMock { get; } = new();
    public Mock<IBlobContainerManager> AnimalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IBlobContainerManager> OriginalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IUserManager> UserManagerMock { get; } = new();

    public HttpClient CreateClient(string role = UserRoles.Administrator) => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{role}"));

    protected override void ConfigureServices(IServiceCollection services)
    {
        // add test auth handler
        services.AddAuthentication(TestAuthHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

        // remove selected services
        services.RemoveAll<IEventPublisher>()
            .RemoveAll<IBlobContainerManager>()
            .RemoveAll<IUserManager>();

        // replace with mocked services
        services.AddScoped<IEventPublisher>(_ => EventPublisherMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages,
                (_, _) => AnimalImagesBlobContainerManagerMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages,
                (_, _) => OriginalImagesBlobContainerManagerMock.Object)
            .AddScoped<IUserManager>(_ => UserManagerMock.Object);
    }
}
