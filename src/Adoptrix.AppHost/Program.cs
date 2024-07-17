using Microsoft.Extensions.Hosting;

namespace Adoptrix.AppHost;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var sqlServerPassword = builder.AddParameter("sql-server-password");

        var database = builder.AddSqlServer("sql-server", sqlServerPassword)
            .WithDataVolume("adoptrix-sql-server-data")
            .AddDatabase("database", "adoptrix");

        var storage = builder.AddAzureStorage("storage");
        if (builder.Environment.IsDevelopment())
        {
            storage.RunAsEmulator(resourceBuilder => resourceBuilder.WithImageTag("latest"));
        }

        var blobStorage = storage.AddBlobs("blob-storage");
        var queueStorage = storage.AddQueues("queue-storage");

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
}
