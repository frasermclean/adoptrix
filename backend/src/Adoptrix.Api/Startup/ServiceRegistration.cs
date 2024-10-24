using System.Text.Json;
using System.Text.Json.Serialization;
using Adoptrix.Api.Extensions;
using Adoptrix.Api.Options;
using Adoptrix.Api.Security;
using Adoptrix.Api.Services;
using Adoptrix.Core;
using Adoptrix.Persistence.Services;
using Adoptrix.ServiceDefaults;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
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
        builder.AddServiceDefaults();
        //.AddPersistence();

        builder.Services
            .AddPersistence(builder.Configuration)
            .AddAuthentication(builder.Configuration)
            .AddUserManagement(builder.Configuration)
            .AddRequestContext()
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
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
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

    private static IServiceCollection AddUserManagement(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<UserManagerOptions>()
            .BindConfiguration(UserManagerOptions.SectionName);

        services.AddSingleton(serviceProvider =>
        {
            var userManagerOptions = serviceProvider.GetRequiredService<IOptions<UserManagerOptions>>();

            var instance = configuration["Authentication:Instance"];
            var tenantId = configuration["Authentication:TenantId"];
            var clientId = userManagerOptions.Value.ClientId;
            var clientSecret = userManagerOptions.Value.ClientSecret;

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret,
                new ClientSecretCredentialOptions
                {
                    AuthorityHost = new Uri(instance!)
                });

            var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>()
                .CreateClient(nameof(GraphServiceClient));

            return new GraphServiceClient(httpClient, credential);
        });

        return services.AddScoped<IUserManager, UserManager>();
    }

    private static IServiceCollection AddRequestContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor()
            .AddScoped<IRequestContext>(serviceProvider =>
            {
                var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

                return new RequestContext
                {
                    UserId = httpContext?.User.GetUserId() ?? Guid.Empty
                };
            });

        return services;
    }
}
