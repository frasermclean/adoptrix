using Adoptrix.Application.Services;
using Adoptrix.Infrastructure.Storage.Services;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adoptrix.Infrastructure.Storage.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureStorage(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddScoped<IAnimalImageManager, AnimalImageManager>();
        services.AddSingleton<IEventPublisher, EventPublisher>();

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

        services.AddKeyedScoped<BlobContainerClient>(BlobContainerKeys.AnimalImages, (provider, _)
            => provider.GetRequiredService<BlobServiceClient>()
                .GetBlobContainerClient("animal-images"));

        // animal deleted queue
        services.AddKeyedSingleton<QueueClient>(QueueKeys.AnimalDeleted, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(configuration.GetValue<string>("AzureStorage:QueueNames:AnimalDeleted")));

        // animal image added queue
        services.AddKeyedSingleton<QueueClient>(QueueKeys.AnimalImageAdded, (provider, _)
            => provider.GetRequiredService<QueueServiceClient>()
                .GetQueueClient(configuration.GetValue<string>("AzureStorage:QueueNames:AnimalImageAdded")));

        return services;
    }
}