using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.AppHost;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args)
            .AddSqlServerWithDatabase(out var database)
            .AddAzureStorage(out var blobStorage, out var queueStorage);

        var api = builder.AddProject<Projects.Adoptrix_Api>("adoptrix-api")
            .WithReference(database)
            .WithReference(blobStorage)
            .WithReference(queueStorage);

        builder.AddProject<Projects.Adoptrix_Initializer>("initializer")
            .WithReference(database);

        builder.AddProject<Projects.Adoptrix_Web>("adoptrix-web")
            .WithExternalHttpEndpoints()
            .WithReference(api);

        builder.Build().Run();
    }

    private static IDistributedApplicationBuilder AddSqlServerWithDatabase(
        this IDistributedApplicationBuilder builder,
        out IResourceBuilder<SqlServerDatabaseResource> database)
    {
        var sqlServerPassword = builder.AddParameter("sql-server-password");

        database = builder.AddSqlServer("sql-server", sqlServerPassword)
            .WithBindMount("../../deps/scripts", "/usr/config")
            .WithDataVolume("adoptrix-sql-server-data")
            .WithEntrypoint("/usr/config/entrypoint.sh")
            .AddDatabase("database", "adoptrix");

        return builder;
    }

    private static IDistributedApplicationBuilder AddAzureStorage(
        this IDistributedApplicationBuilder builder,
        out IResourceBuilder<AzureBlobStorageResource> blobStorage,
        out IResourceBuilder<AzureQueueStorageResource> queueStorage)
    {
        var storage = builder.AddAzureStorage("storage")
            .RunAsEmulator(resourceBuilder =>
            {
                resourceBuilder.WithDataVolume("adoptrix-storage-data");
                resourceBuilder.WithImageTag("latest");
            });

        blobStorage = storage.AddBlobs("blob-storage");
        queueStorage = storage.AddQueues("queue-storage");

        return builder;
    }
}
