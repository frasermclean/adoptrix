using System.Net.Http.Headers;
using Adoptrix.Api.Security;
using Adoptrix.Api.Services;
using Adoptrix.Persistence;
using Adoptrix.Persistence.Services;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Adoptrix.Api.Tests;

public class ApiFixture : AppFixture<Program>
{
    private readonly MsSqlContainer databaseContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithWaitStrategy(Wait
            .ForUnixContainer() // needed until https://github.com/testcontainers/testcontainers-dotnet/issues/1220 is resolved
            .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .Build();

    public Mock<IAnimalsRepository> AnimalsRepositoryMock { get; } = new();
    public Mock<IBreedsRepository> BreedsRepositoryMock { get; } = new();
    public Mock<IEventPublisher> EventPublisherMock { get; } = new();
    public Mock<IBlobContainerManager> AnimalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IBlobContainerManager> OriginalImagesBlobContainerManagerMock { get; } = new();
    public Mock<IUsersService> UsersServiceMock { get; } = new();

    public HttpClient UserClient => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{RoleNames.User}"));

    public HttpClient AdminClient => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{RoleNames.Administrator}"));

    protected override async Task PreSetupAsync()
    {
        await databaseContainer.StartAsync();
    }

    protected override void ConfigureApp(IWebHostBuilder hostBuilder)
    {
        hostBuilder.UseSetting("ConnectionStrings:database", databaseContainer.GetConnectionString());
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        // add test auth handler
        services.AddAuthentication(TestAuthHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

        // remove selected services
        services.RemoveAll<IAnimalsRepository>()
            .RemoveAll<IBreedsRepository>()
            .RemoveAll<IEventPublisher>()
            .RemoveAll<IBlobContainerManager>()
            .RemoveAll<IUsersService>();

        // replace with mocked services
        services.AddScoped<IAnimalsRepository>(_ => AnimalsRepositoryMock.Object)
            .AddScoped<IBreedsRepository>(_ => BreedsRepositoryMock.Object)
            .AddScoped<IEventPublisher>(_ => EventPublisherMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages,
                (_, _) => AnimalImagesBlobContainerManagerMock.Object)
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages,
                (_, _) => OriginalImagesBlobContainerManagerMock.Object)
            .AddScoped<IUsersService>(_ => UsersServiceMock.Object);
    }

    protected override async Task SetupAsync()
    {
        await using var dbContext = Services.GetRequiredService<AdoptrixDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
