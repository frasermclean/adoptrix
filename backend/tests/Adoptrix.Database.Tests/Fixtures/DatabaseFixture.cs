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

    public IAnimalsService? AnimalsService { get; private set; }
    public IBreedsService? BreedsService { get; private set; }
    public ISpeciesRepository? SpeciesRepository { get; private set; }

    public async Task InitializeAsync()
    {
        await container.StartAsync();
        var connectionString = container.GetConnectionString();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(CreateConfiguration(connectionString))
            .AddDatabaseServices()
            .AddLogging()
            .BuildServiceProvider();

        // ensure the database is created
        await serviceProvider.GetRequiredService<AdoptrixDbContext>()
            .Database
            .EnsureCreatedAsync();

        AnimalsService = serviceProvider.GetRequiredService<IAnimalsService>();
        BreedsService = serviceProvider.GetRequiredService<IBreedsService>();
        SpeciesRepository = serviceProvider.GetRequiredService<ISpeciesRepository>();
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
