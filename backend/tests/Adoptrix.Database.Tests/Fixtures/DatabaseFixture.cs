using Adoptrix.Application.Services;
using Adoptrix.Database.DependencyInjection;
using Adoptrix.Database.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Adoptrix.Database.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    private IServiceProvider? serviceProvider;

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        var connectionString = container.GetConnectionString();

        serviceProvider = new ServiceCollection()
            .AddSingleton(CreateConfiguration(connectionString))
            .AddDatabaseServices()
            .AddLogging()
            .BuildServiceProvider();

        // ensure the database is created
        await serviceProvider.GetRequiredService<AdoptrixDbContext>()
            .Database
            .EnsureCreatedAsync();
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

        return new RepositoryCollection
        {
            AnimalsRepository = scope.ServiceProvider.GetRequiredService<IAnimalsRepository>(),
            BreedsRepository = scope.ServiceProvider.GetRequiredService<IBreedsRepository>(),
            SpeciesRepository = scope.ServiceProvider.GetRequiredService<ISpeciesRepository>()
        };
    }

    private static IConfiguration CreateConfiguration(string connectionString)
        => new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "Database:ConnectionString", connectionString
                }
            })
            .Build();
}
