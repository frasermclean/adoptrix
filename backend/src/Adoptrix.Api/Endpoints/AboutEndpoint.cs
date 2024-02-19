using Adoptrix.Api.Contracts.Responses;

namespace Adoptrix.Api.Endpoints;

public static class AboutEndpoint
{
    private static readonly AboutResponse Response = new(
        Version: Environment.GetEnvironmentVariable("APP_VERSION") ?? "Unknown",
        Environment: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown");

    public static AboutResponse Execute() => Response;
}
