using Adoptrix.Application.Services.Repositories;
using Adoptrix.Infrastructure.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Infrastructure.Data.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureData(this IServiceCollection services)
    {
        return services
            .AddDbContext<AdoptrixDbContext>()
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
    }
}