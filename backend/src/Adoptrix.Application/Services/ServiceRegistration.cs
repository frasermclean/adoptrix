using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IHashGenerator, HashGenerator>();
        services.AddSingleton<IImageProcessor, ImageProcessor>();

        return services;
    }
}