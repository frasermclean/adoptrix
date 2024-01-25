using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
            .AddDbContext<AdoptrixDbContext>()
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
    }
}