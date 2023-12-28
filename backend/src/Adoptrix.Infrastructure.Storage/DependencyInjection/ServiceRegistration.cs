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
            .ValidateDataAnnotations();

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
        services.AddKeyedScoped<BlobContainerClient>(BlobContainerNames.AnimalImages, (provider, _)
            => provider.GetRequiredService<BlobServiceClient>()
                .GetBlobContainerClient(BlobContainerNames.AnimalImages));

        return services;
    }

    private static IServiceCollection AddStorageQueueClients(this IServiceCollection services)
    {
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
