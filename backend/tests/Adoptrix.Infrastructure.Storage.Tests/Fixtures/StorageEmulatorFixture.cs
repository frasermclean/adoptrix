using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;

namespace Adoptrix.Infrastructure.Storage.Tests.Fixtures;

public class StorageEmulatorFixture : IAsyncLifetime
{
    private readonly AzuriteContainer container = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
        .WithPortBinding(BlobContainerPort, true)
        .WithPortBinding(QueueContainerPort, true)
        .WithPortBinding(TableContainerPort, true)
        .Build();

    private readonly IServiceProvider serviceProvider;

    public BlobContainerClient BlobContainerClient => serviceProvider.GetRequiredService<BlobContainerClient>();

    private const int BlobContainerPort = 10000;
    private const int QueueContainerPort = 10001;
    private const int TableContainerPort = 10002;

    public StorageEmulatorFixture()
    {
        serviceProvider = new ServiceCollection()
            .AddSingleton<ConnectionStringBuilder>()
            .AddSingleton<BlobContainerClient>(provider =>
            {
                var connectionStringBuilder = provider.GetRequiredService<ConnectionStringBuilder>();
                return new BlobContainerClient(connectionStringBuilder.ConnectionString,
                    BlobContainerNames.AnimalImages);
            })
            .BuildServiceProvider();
    }

    public async Task InitializeAsync()
    {
        await container.StartAsync();

        // update connection string builder with runtime mapped ports
        var connectionStringBuilder = serviceProvider.GetRequiredService<ConnectionStringBuilder>();
        connectionStringBuilder.BlobPort = container.GetMappedPublicPort(BlobContainerPort);
        connectionStringBuilder.QueuePort =  container.GetMappedPublicPort(QueueContainerPort);
        connectionStringBuilder.TablePort = container.GetMappedPublicPort(TableContainerPort);

        await BlobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
    }

    public async Task DisposeAsync()
    {
        await container.StopAsync();
    }
}