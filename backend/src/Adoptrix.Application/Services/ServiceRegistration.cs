using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Adoptrix.Application.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IHashGenerator, HashGenerator>();
        services.AddSingleton<ISqidConverter, SqidConverter>();
        services.AddOptions<SqidsOptions>()
            .Bind(configuration.GetSection("Sqids"));

        return services;
    }
}