using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services.Repositories;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
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
                // TODO: Refactor connection string when moving to production
                builder.AddBlobServiceClient("UseDevelopmentStorage=true");
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