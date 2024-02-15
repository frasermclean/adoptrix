using System.Net.Http.Headers;
using Adoptrix.Api.Tests.Mocks;
using Adoptrix.Application.Services;
using Adoptrix.Application.Services.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adoptrix.Api.Tests.Fixtures;

public class ApiFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove the real repositories and replace them with mocks
            services.RemoveAll<IAnimalsRepository>();
            services.RemoveAll<IBreedsRepository>();
            services.RemoveAll<ISpeciesRepository>();
            services.AddScoped<IAnimalsRepository, MockAnimalsRepository>();
            services.AddScoped<IBreedsRepository, MockBreedsRepository>();
            services.AddScoped<ISpeciesRepository, MockSpeciesRepository>();

            // remove the real event publisher and replace it with a mock
            services.RemoveAll<IEventPublisher>();
            services.AddScoped<IEventPublisher, MockEventPublisher>();

            // add test auth handler
            services.AddAuthentication(TestAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        // configure the client to use the test auth scheme
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.SchemeName);
    }
}
