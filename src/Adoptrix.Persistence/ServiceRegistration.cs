using Adoptrix.Persistence.Services;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Persistence;

public static class ServiceRegistration
{
    /// <summary>
    /// Add database and storage services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventPublisher, EventPublisher>()
            .AddDatabaseServices(configuration)
            .AddAzureStorageServices(configuration);

        return services;
    }

    internal static IServiceCollection AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextFactory<AdoptrixDbContext>(optionsBuilder =>
            {
                var connectionString = configuration[AdoptrixDbContext.ConnectionStringKey];
                optionsBuilder.UseSqlServer(connectionString);
            })
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }

    internal static IServiceCollection AddAzureStorageServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAzureClients(builder =>
        {
            builder.ConfigureDefaults(options =>
            {
                options.Diagnostics.IsLoggingEnabled = false;
            });

            var connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString");

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
        services.AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages, (provider, _)
            => new BlobContainerManager(provider.GetRequiredService<BlobServiceClient>(),
                BlobContainerNames.AnimalImages));

        // original images blob container
        services.AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages, (provider, _)
            => new BlobContainerManager(provider.GetRequiredService<BlobServiceClient>(),
                BlobContainerNames.OriginalImages));

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
