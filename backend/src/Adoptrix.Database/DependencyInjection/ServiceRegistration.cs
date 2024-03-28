using Adoptrix.Application.Services;
using Adoptrix.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Database.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services) => services
        .AddDbContext<AdoptrixDbContext>((serviceProvider, builder) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetValue<string>("Database:ConnectionString");
            builder.UseSqlServer(connectionString);
        }, optionsLifetime: ServiceLifetime.Singleton)
        .AddScoped<IAnimalsRepository, AnimalsRepository>()
        .AddScoped<IBreedsRepository, BreedsRepository>()
        .AddScoped<ISpeciesRepository, SpeciesRepository>()
        .AddScoped<IBatchManager, BatchManager>();
}
