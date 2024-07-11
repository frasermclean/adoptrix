using Adoptrix.Core.Abstractions;
using Adoptrix.Storage.DependencyInjection;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;

namespace Adoptrix.Storage.Tests.Fixtures;

public class StorageEmulatorFixture : IAsyncLifetime
{
    private readonly AzuriteContainer container = new AzuriteBuilder()
        .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
        .WithPortBinding(BlobContainerPort, true)
        .WithPortBinding(QueueContainerPort, true)
        .WithPortBinding(TableContainerPort, true)
        .Build();

    public IServiceProvider? ServiceProvider { get; private set; }

    public IBlobContainerManager? BlobContainerManager =>
        ServiceProvider?.GetRequiredKeyedService<IBlobContainerManager>(BlobContainerNames.AnimalImages);

    public QueueClient? AnimalDeletedQueueClient =>
        ServiceProvider?.GetRequiredKeyedService<QueueClient>(QueueNames.AnimalDeleted);

    public QueueClient? AnimalImageAddedQueueClient =>
        ServiceProvider?.GetRequiredKeyedService<QueueClient>(QueueNames.AnimalImageAdded);

    private const int BlobContainerPort = 10000;
    private const int QueueContainerPort = 10001;
    private const int TableContainerPort = 10002;

    public async Task InitializeAsync()
    {
        await container.StartAsync();

        // update connection string builder with runtime mapped ports
        var connectionStringBuilder = new ConnectionStringBuilder
        {
            BlobPort = container.GetMappedPublicPort(BlobContainerPort),
            QueuePort = container.GetMappedPublicPort(QueueContainerPort),
            TablePort = container.GetMappedPublicPort(TableContainerPort)
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {
                    "AzureStorage:ConnectionString", connectionStringBuilder.ConnectionString
                }
            })
            .Build();

        ServiceProvider = new ServiceCollection()
            .AddStorageServices(configuration)
            .BuildServiceProvider();

        await InitializeBlobContainersAsync(ServiceProvider.GetRequiredService<BlobServiceClient>());
        await InitializeQueuesAsync(ServiceProvider.GetRequiredService<QueueServiceClient>());
    }

    private static async Task InitializeBlobContainersAsync(BlobServiceClient serviceClient)
    {
        var containerClient = serviceClient.GetBlobContainerClient(BlobContainerNames.AnimalImages);
        await containerClient.CreateAsync(PublicAccessType.Blob);
    }

    private static async Task InitializeQueuesAsync(QueueServiceClient serviceClient)
    {
        var queuesToCreates = new[]
        {
            QueueNames.AnimalDeleted, QueueNames.AnimalImageAdded
        };

        foreach (var queueName in queuesToCreates)
        {
            var queueClient = serviceClient.GetQueueClient(queueName);
            await queueClient.CreateAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await container.StopAsync();
    }
}
