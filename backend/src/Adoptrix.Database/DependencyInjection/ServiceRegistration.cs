using Adoptrix.Core.Abstractions;
using Adoptrix.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Database.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddDbContextFactory<AdoptrixDbContext>(optionsBuilder =>
            {
                var connectionString = configuration[AdoptrixDbContext.ConnectionStringKey];
                optionsBuilder.UseSqlServer(connectionString);
            })
            .AddScoped<IAnimalsRepository, AnimalsRepository>()
            .AddScoped<IBreedsRepository, BreedsRepository>()
            .AddScoped<ISpeciesRepository, SpeciesRepository>();
}
