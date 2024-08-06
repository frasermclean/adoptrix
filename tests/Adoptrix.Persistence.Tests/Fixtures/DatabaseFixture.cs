using Adoptrix.Persistence.Services;
using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Adoptrix.Persistence.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithWaitStrategy(Wait.ForUnixContainer() // needed until https://github.com/testcontainers/testcontainers-dotnet/issues/1220 is resolved
            .UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .Build();

    private IServiceProvider? serviceProvider;

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        var connectionString = container.GetConnectionString();
        var configuration = CreateConfiguration(connectionString);

        serviceProvider = new ServiceCollection()
            .AddDatabaseServices(configuration)
            .AddLogging()
            .BuildServiceProvider();

        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AdoptrixDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await container.StopAsync();
    }

    /// <summary>
    /// Get a collection of scoped repositories for use in tests.
    /// </summary>
    /// <exception cref="InvalidOperationException">Will occur if service provider is accessed before
    /// class has been properly initialized.</exception>
    public RepositoryCollection GetRepositoryCollection()
    {
        var scope = serviceProvider?.CreateScope() ??
                    throw new InvalidOperationException("Service provider is not initialized");

        return new RepositoryCollection(
            scope.ServiceProvider.GetRequiredService<IAnimalsRepository>(),
            scope.ServiceProvider.GetRequiredService<IBreedsRepository>(),
            scope.ServiceProvider.GetRequiredService<ISpeciesRepository>()
        );
    }

    private static IConfiguration CreateConfiguration(string connectionString)
        => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "ConnectionStrings:database", connectionString
                }
            })
            .Build();
}
