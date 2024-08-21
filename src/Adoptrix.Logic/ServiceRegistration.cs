using Adoptrix.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Logic;

public static class ServiceRegistration
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalsService, AnimalsService>();

        return services;
    }
}
