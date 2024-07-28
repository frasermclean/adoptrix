using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Services;
using Adoptrix.Persistence.Services;
using Adoptrix.ServiceDefaults;
using Azure.Identity;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace Adoptrix.Api.Startup;

public static class ServiceRegistration
{
    /// <summary>
    /// Registers services within the dependency injection container.
    /// </summary>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults()
            .AddPersistence();

        builder.Services
            .AddAuthentication(builder.Configuration)
            .AddFastEndpoints()
            .AddUsersService(builder.Configuration);

        // json serialization options
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder => policyBuilder
                    .WithOrigins(["http://localhost:5157", "https://localhost:7157"])
                    .AllowAnyMethod());
            });
        }

        return builder;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
            {
                configuration.Bind("Authentication", jwtBearerOptions);
                jwtBearerOptions.TokenValidationParameters.NameClaimType = ClaimConstants.Name;
            }, microsoftIdentityOptions => { configuration.Bind("Authentication", microsoftIdentityOptions); });

        services.AddAuthorizationBuilder()
            .AddDefaultPolicy("DefaultPolicy", builder => { builder.RequireScope("access"); });

        return services;
    }

    private static IServiceCollection AddUsersService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider =>
        {
            // read values from configuration
            var instance = configuration["Authentication:Instance"];
            var tenantId = configuration["Authentication:TenantId"];
            var clientId = configuration["UserManager:ClientId"];
            var clientSecret = configuration["UserManager:ClientSecret"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret, new ClientSecretCredentialOptions
            {
                AuthorityHost = new Uri(instance!)
            });

            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(nameof(GraphServiceClient));

            return new GraphServiceClient(httpClient, credential);
        });

        return services.AddScoped<IUsersService, UsersService>();
    }
}
