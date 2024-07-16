var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Adoptrix_Api>("adoptrix-api");

builder.AddProject<Projects.Adoptrix_Web>("adoptrix-web")
    .WithExternalHttpEndpoints()
    .WithReference(api);

builder.Build().Run();
