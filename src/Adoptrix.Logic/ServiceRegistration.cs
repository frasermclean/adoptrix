using Adoptrix.Logic.Services;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;

namespace Adoptrix.Logic;

public static class ServiceRegistration
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUsersService(configuration)
            .AddScoped<IAnimalsService, AnimalsService>()
            .AddScoped<IBreedsService, BreedsService>()
            .AddScoped<ISpeciesService, SpeciesService>()
            .AddScoped<IAnimalImagesManager, AnimalImagesManager>()
            .AddSingleton<IImageProcessor, ImageProcessor>();

        return services;
    }

    private static IServiceCollection AddUsersService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider =>
        {
            // read values from configuration
            var instance = configuration["Authentication:Instance"];
            var tenantId = configuration["Authentication:TenantId"];
            var clientId = configuration["UserManager:ClientId"];
            var clientSecret = configuration["UserManager:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret,
                new ClientSecretCredentialOptions
                {
                    AuthorityHost = new Uri(instance!)
                });

            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(nameof(GraphServiceClient));

            return new GraphServiceClient(httpClient, credential);
        });

        return services.AddScoped<IUsersService, UsersService>();
    }
}
