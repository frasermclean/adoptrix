using System.Net.Http.Headers;
using Adoptrix.Core;
using Adoptrix.Initializer;
using Adoptrix.Logic.Services;
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

    public HttpClient AdminClient => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{UserRole.Administrator}"));

    public HttpClient UserClient => CreateClient(httpClient =>
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue($"{TestAuthHandler.SchemeName}-{UserRole.User}"));

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
            .RemoveAll<IUserManager>();

        // replace with mocked services
        services.AddScoped<IEventPublisher>(_ => Mock.Of<IEventPublisher>())
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages,
                (_, _) => Mock.Of<IBlobContainerManager>())
            .AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages,
                (_, _) => Mock.Of<IBlobContainerManager>())
            .AddScoped<IUserManager>(_ => Mock.Of<IUserManager>());
    }

    protected override async Task SetupAsync()
    {
        var dbContext = Services.GetRequiredService<AdoptrixDbContext>();

        var wasCreated = await dbContext.Database.EnsureCreatedAsync();
        if (wasCreated)
        {
            dbContext.Species.AddRange(SeedData.Species);
            dbContext.Breeds.AddRange(SeedData.Breeds);
            dbContext.Animals.AddRange(SeedData.Animals);
            await dbContext.SaveChangesAsync();
        }
    }
}
