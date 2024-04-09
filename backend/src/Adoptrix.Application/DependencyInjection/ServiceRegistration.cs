using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalsService, AnimalsService>();
        services.AddScoped<IBreedsService, BreedsService>();
        services.AddScoped<ISpeciesService, SpeciesService>();
        services.AddSingleton<IImageProcessor, ImageProcessor>();

        return services;
    }
}
