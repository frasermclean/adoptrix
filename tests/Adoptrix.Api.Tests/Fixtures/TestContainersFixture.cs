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

namespace Adoptrix.Api.Tests.Fixtures;

public class TestContainersFixture : AppFixture<Program>
{
    private readonly MsSqlContainer databaseContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithWaitStrategy(Wait
            .ForUnixContainer() // needed until https://github.com/testcontainers/testcontainers-dotnet/issues/1220 is resolved
            .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .Build();

    public HttpClient CreateClient(string role = RoleNames.Administrator) => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{role}"));

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
        services.RemoveAll<IEventPublisher>()
            .RemoveAll<IBlobContainerManager>()
            .RemoveAll<IUsersService>();

        // replace with mocked services
        services.AddScoped<IEventPublisher>(_ => Mock.Of<IEventPublisher>())
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages,
                (_, _) => Mock.Of<IBlobContainerManager>())
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages,
                (_, _) => Mock.Of<IBlobContainerManager>())
            .AddScoped<IUsersService>(_ => Mock.Of<IUsersService>());
    }

    protected override async Task SetupAsync()
    {
        await using var dbContext = Services.GetRequiredService<AdoptrixDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
