using Adoptrix.Application.Contracts.Requests.Animals;
using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddMediatR(configuration => { configuration.RegisterServicesFromAssemblyContaining<AddAnimalRequest>(); })
            .AddSingleton<IImageProcessor, ImageProcessor>();
    }
}
