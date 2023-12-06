using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;

namespace Adoptrix.Api.Tests;

public class ApiTestFixture : TestFixture<Program>
{
    public ApiTestFixture(IMessageSink messageSink)
        : base(messageSink)
    {
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.RemoveAll<IAnimalsRepository>();
        services.AddScoped<IAnimalsRepository, MockAnimalsRepository>();
    }
}