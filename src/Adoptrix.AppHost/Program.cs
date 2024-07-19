using Aspire.Hosting.Azure;

namespace Adoptrix.AppHost;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args)
            .AddSqlServerWithDatabase(out var database)
            .AddAzureStorage(out var blobStorage, out var queueStorage);

        var appInsights = builder.AddConnectionString("ApplicationInsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");

        var api = builder.AddProject<Projects.Adoptrix_Api>("adoptrix-api")
            .WithReference(database)
            .WithReference(blobStorage)
            .WithReference(queueStorage)
            .WithReference(appInsights);

        builder.AddProject<Projects.Adoptrix_Initializer>("initializer")
            .WithReference(database)
            .WithReference(blobStorage)
            .WithReference(queueStorage);

        builder.AddProject<Projects.Adoptrix_Web>("adoptrix-web")
            .WithExternalHttpEndpoints()
            .WithReference(api)
            .WithReference(appInsights);

        builder.Build().Run();
    }

    private static IDistributedApplicationBuilder AddSqlServerWithDatabase(
        this IDistributedApplicationBuilder builder,
        out IResourceBuilder<SqlServerDatabaseResource> database)
    {
        var sqlServerPassword = builder.AddParameter("sql-server-password");

        database = builder.AddSqlServer("sql-server", sqlServerPassword)
            .WithDataVolume("adoptrix-sql-server-data")
            .AddDatabase("database", "adoptrix");

        return builder;
    }

    private static IDistributedApplicationBuilder AddAzureStorage(
        this IDistributedApplicationBuilder builder,
        out IResourceBuilder<AzureBlobStorageResource> blobStorage,
        out IResourceBuilder<AzureQueueStorageResource> queueStorage)
    {
        var storage = builder.AddAzureStorage("storage");

        // run as emulator in local development
        if (builder.ExecutionContext.IsRunMode)
        {
            storage.RunAsEmulator(resourceBuilder =>
            {
                resourceBuilder.WithDataVolume("adoptrix-storage-data");
                resourceBuilder.WithImageTag("latest");
            });
        }

        blobStorage = storage.AddBlobs("blob-storage");
        queueStorage = storage.AddQueues("queue-storage");

        return builder;
    }
}
