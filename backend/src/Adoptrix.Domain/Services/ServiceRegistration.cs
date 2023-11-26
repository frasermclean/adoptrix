using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Domain.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventQueueService, EventQueueService>();

        return services;
    }
}