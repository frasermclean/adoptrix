using Adoptrix.Persistence.Interceptors;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using EntityFramework.Exceptions.SqlServer;
using Gridify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.Persistence.Services;

public static class ServiceRegistration
{
    /// <summary>
    /// Add database and storage services to the dependency injection container.
    /// </summary>
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<AdoptrixDbContext>("database",
            configureDbContextOptions: optionsBuilder =>
            {
                optionsBuilder.AddInterceptors(new LastModifiedInterceptor());
                optionsBuilder.UseExceptionProcessor();
            });
        builder.AddAzureBlobClient("blob-storage");
        builder.AddAzureQueueClient("queue-storage");

        builder.Services.AddProjectServices()
            .AddBlobServices()
            .AddQueueServices();

        ConfigureGridify();

        return builder;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProjectServices()
            .AddDatabaseServices(configuration)
            .AddAzureStorageServices(configuration);

        ConfigureGridify();

        return services;
    }

    private static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventPublisher, EventPublisher>();

        return services;
    }

    internal static IServiceCollection AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextFactory<AdoptrixDbContext>(optionsBuilder =>
        {
            var connectionString = configuration.GetConnectionString("database");
            optionsBuilder.UseSqlServer(connectionString)
                .AddInterceptors(new LastModifiedInterceptor())
                .UseExceptionProcessor();
        });

        return services;
    }

    internal static IServiceCollection AddAzureStorageServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAzureClients(builder =>
        {
            var credential = TokenCredentialFactory.Create();
            builder.UseCredential(credential)
                .ConfigureDefaults(options => { options.Diagnostics.IsLoggingEnabled = false; });

            // add blob service client
            var blobStorageConnectionString = configuration.GetConnectionString("blob-storage")!;
            if (blobStorageConnectionString.StartsWith("https://"))
            {
                builder.AddBlobServiceClient(new Uri(blobStorageConnectionString));
            }
            else
            {
                builder.AddBlobServiceClient(blobStorageConnectionString);
            }

            // add queue service client
            var queueStorageConnectionString = configuration.GetConnectionString("queue-storage")!;
            if (queueStorageConnectionString.StartsWith("https://"))
            {
                builder.AddQueueServiceClient(new Uri(queueStorageConnectionString));
            }
            else
            {
                builder.AddQueueServiceClient(queueStorageConnectionString);
            }
        });

        services.AddBlobServices()
            .AddQueueServices();

        return services;
    }

    private static void ConfigureGridify()
    {
        GridifyGlobalConfiguration.EnableEntityFrameworkCompatibilityLayer();
    }

    private static IServiceCollection AddBlobServices(this IServiceCollection services)
    {
        // animal images blob container
        services.AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.AnimalImages, (provider, _)
            => new BlobContainerManager(provider.GetRequiredService<BlobServiceClient>(),
                BlobContainerNames.AnimalImages));

        // original images blob container
        services.AddKeyedSingleton<IBlobContainerManager>(BlobContainerNames.OriginalImages, (provider, _)
            => new BlobContainerManager(provider.GetRequiredService<BlobServiceClient>(),
                BlobContainerNames.OriginalImages));

        return services;
    }

    private static void AddQueueServices(this IServiceCollection services)
    {
        // animal deleted queue
        services.AddKeyedSingleton<QueueClient>(QueueNames.AnimalDeleted, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(QueueNames.AnimalDeleted));

        // animal image added queue
        services.AddKeyedSingleton<QueueClient>(QueueNames.AnimalImageAdded, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(QueueNames.AnimalImageAdded));
    }
}
