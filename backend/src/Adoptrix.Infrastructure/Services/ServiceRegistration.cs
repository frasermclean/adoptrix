using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services.Repositories;
using Azure.Identity;
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
            .AddDataServices()
            .AddScoped<IAnimalImageManager, AnimalImageManager>()
            .AddAzureClients(configuration, environment);

        return services;
    }

    private static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<AdoptrixDbContext>()
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
    }

    private static IServiceCollection AddAzureClients(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddAzureClients(builder =>
        {
            if (environment.IsDevelopment())
            {
                builder.AddBlobServiceClient("UseDevelopmentStorage=true");
                builder.AddQueueServiceClient("UseDevelopmentStorage=true");
                return;
            }

            builder.AddBlobServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:BlobEndpoint")!));
            builder.AddQueueServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:QueueEndpoint")!));
            builder.UseCredential(new DefaultAzureCredential());
        });

        services.AddKeyedScoped<BlobContainerClient>(AnimalImageManager.ContainerName, (provider, _)
            => provider.GetRequiredService<BlobServiceClient>()
                .GetBlobContainerClient(AnimalImageManager.ContainerName));

        return services;
    }
}