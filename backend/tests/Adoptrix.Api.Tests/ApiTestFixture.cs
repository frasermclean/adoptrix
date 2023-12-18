using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests;

public class ApiTestFixture(IMessageSink messageSink)
    : TestFixture<Program>(messageSink)
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        // remove the real repositories and replace them with mocks
        services.RemoveAll<IAnimalsRepository>();
        services.RemoveAll<IBreedsRepository>();
        services.RemoveAll<ISpeciesRepository>();
        services.AddScoped<IAnimalsRepository, MockAnimalsRepository>();
        services.AddScoped<IBreedsRepository, MockBreedsRepository>();
        services.AddScoped<ISpeciesRepository, MockSpeciesRepository>();
    }
}