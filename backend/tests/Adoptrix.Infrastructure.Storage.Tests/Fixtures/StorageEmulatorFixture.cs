using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Testcontainers.Azurite;

namespace Adoptrix.Infrastructure.Storage.Tests.Fixtures;

public class StorageEmulatorFixture : IAsyncLifetime
{
    private readonly AzuriteContainer container;
    public BlobContainerClient? BlobContainerClient { get; private set; }

    private const int BlobContainerPort = 10000;
    private const int QueueContainerPort = 10001;
    private const int TableContainerPort = 10002;

    public StorageEmulatorFixture()
    {
        container = new AzuriteBuilder()
            .WithImage("mcr.microsoft.com/azure-storage/azurite:latest")
            .WithPortBinding(BlobContainerPort, true)
            .WithPortBinding(QueueContainerPort, true)
            .WithPortBinding(TableContainerPort, true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await container.StartAsync();

        var blobPort = container.GetMappedPublicPort(BlobContainerPort);
        var queuePort = container.GetMappedPublicPort(QueueContainerPort);
        var tablePort = container.GetMappedPublicPort(TableContainerPort);
        var connectionString =
            $"DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:{blobPort}/devstoreaccount1;QueueEndpoint=http://127.0.0.1:{queuePort}/devstoreaccount1;TableEndpoint=http://127.0.0.1:{tablePort}/devstoreaccount1;";

        BlobContainerClient = new BlobContainerClient(connectionString, "animal-images");
        await BlobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
    }

    public async Task DisposeAsync()
    {
        await container.StopAsync();
    }
}