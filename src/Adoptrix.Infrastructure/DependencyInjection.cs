﻿using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services;
using Adoptrix.Infrastructure.Services.Repositories;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services
            .AddDbContext<AdoptrixDbContext>()
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IAnimalImageUploader, AnimalImageUploader>()
            .AddKeyedScoped<BlobContainerClient>("animal-images", (provider, _) =>
            {
                var serviceClient = provider.GetRequiredService<BlobServiceClient>();
                return serviceClient.GetBlobContainerClient("animal-images");
            })
            .AddAzureClients(builder =>
            {
                // TODO: Refactor connection string when moving to production
                builder.AddBlobServiceClient("UseDevelopmentStorage=true");
            });

        return services;
    }
}