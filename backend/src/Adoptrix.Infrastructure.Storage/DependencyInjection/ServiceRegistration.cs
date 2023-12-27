using Adoptrix.Application.Services;
using Adoptrix.Infrastructure.Storage.Options;
using Adoptrix.Infrastructure.Storage.Services;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Adoptrix.Infrastructure.Storage.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureStorage(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddScoped<IAnimalImageManager, AnimalImageManager>();
        services.AddSingleton<IEventPublisher, EventPublisher>();

        services.AddOptions<StorageOptions>()
            .BindConfiguration(StorageOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAzureClients(builder =>
        {
            if (environment.IsDevelopment())
            {
                builder.AddBlobServiceClient("UseDevelopmentStorage=true");
                builder.AddQueueServiceClient("UseDevelopmentStorage=true")
                    .ConfigureOptions(options => options.MessageEncoding = QueueMessageEncoding.Base64);

                return;
            }

            builder.AddBlobServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:BlobEndpoint")!));
            builder.AddQueueServiceClient(new Uri(configuration.GetValue<string>("AzureStorage:QueueEndpoint")!))
                .ConfigureOptions(options => options.MessageEncoding = QueueMessageEncoding.Base64);

            builder.UseCredential(new DefaultAzureCredential());
        });

        return services
            .AddBlobContainerClients()
            .AddStorageQueueClients();
    }

    private static IServiceCollection AddBlobContainerClients(this IServiceCollection services)
    {
        return services.AddKeyedScoped<BlobContainerClient>(BlobContainerKeys.AnimalImages, (provider, _)
            =>
        {
            var options = provider.GetRequiredService<IOptions<StorageOptions>>().Value;
            return provider.GetRequiredService<BlobServiceClient>()
                .GetBlobContainerClient(options.BlobContainerNames.AnimalImages);
        });
    }

    private static IServiceCollection AddStorageQueueClients(this IServiceCollection services)
    {
        // animal deleted queue
        services.AddKeyedSingleton<QueueClient>(QueueKeys.AnimalDeleted, (provider, _)
            =>
        {
            var options = provider.GetRequiredService<IOptions<StorageOptions>>().Value;
            return provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(options.QueueNames.AnimalDeleted);
        });

        // animal image added queue
        services.AddKeyedSingleton<QueueClient>(QueueKeys.AnimalImageAdded, (provider, _)
            =>
        {
            var options = provider.GetRequiredService<IOptions<StorageOptions>>().Value;
            return provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(options.QueueNames.AnimalImageAdded);
        });

        return services;
    }
}