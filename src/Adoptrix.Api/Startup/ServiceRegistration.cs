using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Security;
using Adoptrix.Logic;
using Adoptrix.Persistence.Services;
using Adoptrix.ServiceDefaults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
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
            .AddLogicServices(builder.Configuration)
            .AddFastEndpoints();

        // json serialization options
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
                jwtBearerOptions.MapInboundClaims = false;
                jwtBearerOptions.TokenValidationParameters.NameClaimType = ClaimConstants.Name;
                jwtBearerOptions.TokenValidationParameters.RoleClaimType = ClaimConstants.Roles;
            }, microsoftIdentityOptions => { configuration.Bind("Authentication", microsoftIdentityOptions); });

        services.AddTransient<IClaimsTransformation, PermissionsClaimsTransformation>();

        services.AddAuthorizationBuilder()
            .AddDefaultPolicy("DefaultPolicy", builder => { builder.RequireScope("access"); });

        return services;
    }
}
