using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services;
using Adoptrix.Infrastructure.Services.Repositories;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) => services
        .AddScoped<IAnimalImageManager, AnimalImageManager>()
        .AddSingleton<IEventPublisher, EventPublisher>()
        .AddDatabaseServices()
        .AddAzureStorageServices(configuration);

    private static IServiceCollection AddDatabaseServices(this IServiceCollection services) => services
        .AddDbContext<AdoptrixDbContext>()
        .AddScoped<IAnimalsRepository, AnimalsRepository>()
        .AddScoped<IBreedsRepository, BreedsRepository>()
        .AddScoped<ISpeciesRepository, SpeciesRepository>();

    private static IServiceCollection AddAzureStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureClients(builder =>
        {
            var connectionString = configuration.GetConnectionString("AzureStorage");

            // use connection string if it is defined
            if (connectionString is not null)
            {
                builder.AddBlobServiceClient(connectionString);
                builder.AddQueueServiceClient(connectionString)
                    .ConfigureOptions(options => options.MessageEncoding = QueueMessageEncoding.Base64);

                return;
            }

            builder.AddBlobServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:BlobEndpoint")!));
            builder.AddQueueServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:QueueEndpoint")!))
                .ConfigureOptions(options => options.MessageEncoding = QueueMessageEncoding.Base64);

            builder.UseCredential(new DefaultAzureCredential());
        });

        // animal images blob container
        services.AddKeyedSingleton<BlobContainerClient>(BlobContainerNames.AnimalImages, (provider, _)
            => provider.GetRequiredService<BlobServiceClient>()
                .GetBlobContainerClient(BlobContainerNames.AnimalImages));

        // animal deleted queue
        services.AddKeyedSingleton<QueueClient>(QueueNames.AnimalDeleted, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(QueueNames.AnimalDeleted));

        // animal image added queue
        services.AddKeyedSingleton<QueueClient>(QueueNames.AnimalImageAdded, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(QueueNames.AnimalImageAdded));

        return services;
    }
}