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

        var api = builder.AddProject<Projects.Adoptrix_Api>("adoptrix-api")
            .WithHttpsEndpoint(5001)
            .WithReference(database);

        builder.AddProject<Projects.Adoptrix_Web>("adoptrix-web")
            .WithHttpsEndpoint(8443)
            .WithExternalHttpEndpoints()
            .WithReference(api);

        builder.Build().Run();
    }
}
