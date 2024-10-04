using Adoptrix.Logic.Options;
using Adoptrix.Logic.Services;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace Adoptrix.Logic;

public static class ServiceRegistration
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUserManagement(configuration)
            .AddScoped<IAnimalsService, AnimalsService>()
            .AddScoped<IBreedsService, BreedsService>()
            .AddScoped<ISpeciesService, SpeciesService>()
            .AddSingleton<IImageProcessor, ImageProcessor>();

        return services;
    }

    private static IServiceCollection AddUserManagement(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<UserManagerOptions>().BindConfiguration(UserManagerOptions.SectionName);

        services.AddSingleton(serviceProvider =>
        {
            var userManagerOptions = serviceProvider.GetRequiredService<IOptions<UserManagerOptions>>();

            var instance = configuration["Authentication:Instance"];
            var tenantId = configuration["Authentication:TenantId"];
            var clientId = userManagerOptions.Value.ClientId;
            var clientSecret = userManagerOptions.Value.ClientSecret;

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret,
                new ClientSecretCredentialOptions
                {
                    AuthorityHost = new Uri(instance!)
                });

            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(nameof(GraphServiceClient));

            return new GraphServiceClient(httpClient, credential);
        });

        return services.AddScoped<IUserManager, UserManager>();
    }
}
