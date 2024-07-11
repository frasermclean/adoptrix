using System.Reflection;
using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Adoptrix.Application.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        return services
            .AddValidatorsFromAssembly(executingAssembly)
            .AddSingleton<IImageProcessor, ImageProcessor>();
    }
}
