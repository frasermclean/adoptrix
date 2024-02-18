using Adoptrix.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IHashGenerator, HashGenerator>();
        services.AddSingleton<IImageProcessor, ImageProcessor>();

        return services;
    }
}
