using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Adoptrix.Infrastructure.Storage.Tests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public ISpeciesRepository? SpeciesRepository { get; private set; }
    public IBreedsRepository? BreedsRepository { get; private set; }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        var connectionString = container.GetConnectionString();
        var configuration = CreateConfiguration(connectionString);

        var serviceProvider = new ServiceCollection()
            .AddDatabaseServices(configuration)
            .AddLogging()
            .BuildServiceProvider();

        // ensure the database is created
        await serviceProvider.GetRequiredService<AdoptrixDbContext>()
            .Database
            .EnsureCreatedAsync();

        SpeciesRepository = serviceProvider.GetRequiredService<ISpeciesRepository>();
        BreedsRepository = serviceProvider.GetRequiredService<IBreedsRepository>();
    }

    public async Task DisposeAsync()
    {
        await container.StopAsync();
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
