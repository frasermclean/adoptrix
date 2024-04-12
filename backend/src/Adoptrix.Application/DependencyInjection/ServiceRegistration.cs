using Adoptrix.Application.Features.Animals.Commands;
using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddMediatR(configuration => { configuration.RegisterServicesFromAssemblyContaining<AddAnimalCommand>(); })
            .AddSingleton<IImageProcessor, ImageProcessor>();
    }
}
