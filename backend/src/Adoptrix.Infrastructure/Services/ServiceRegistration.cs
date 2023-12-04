using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services.Repositories;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.Infrastructure.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services
            .AddDbContext<AdoptrixDbContext>()
            .AddRepositories()
            .AddScoped<IAnimalImageManager, AnimalImageManager>()
            .AddKeyedScoped<BlobContainerClient>(AnimalImageManager.ContainerName, (provider, _) =>
            {
                var serviceClient = provider.GetRequiredService<BlobServiceClient>();
                return serviceClient.GetBlobContainerClient(AnimalImageManager.ContainerName);
            })
            .AddAzureClients(builder =>
            {
                if (environment.IsDevelopment())
                {
                    builder.AddBlobServiceClient("UseDevelopmentStorage=true");
                    return;
                }

                var blobEndpoint = configuration.GetValue<string>("AzureStorage:BlobEndpoint") ??
                                   throw new InvalidOperationException(
                                       "Missing AzureStorage:BlobEndpoint configuration value.");

                builder.AddBlobServiceClient(new Uri(blobEndpoint));
            });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
    }
}