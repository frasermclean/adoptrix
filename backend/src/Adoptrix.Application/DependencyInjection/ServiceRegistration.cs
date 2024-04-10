using Adoptrix.Application.Contracts.Requests.Breeds;
using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services.AddScoped<IAnimalsService, AnimalsService>()
            .AddScoped<ISpeciesService, SpeciesService>()
            .AddSingleton<IImageProcessor, ImageProcessor>()
            .AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<AddBreedRequest>());
    }
}
