using System.Globalization;
using System.Reflection;
using Adoptrix.Api.Contracts.Responses;

namespace Adoptrix.Api.Endpoints;

public static class AboutEndpoint
{
    private static readonly AboutResponse Response;

    static AboutEndpoint()
    {
        var buildData = GetBuildData(Assembly.GetExecutingAssembly());

        Response = new AboutResponse(
            Version: buildData.Version,
            Environment: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            BuildDate: buildData.BuildDate);
    }

    public static AboutResponse Execute() => Response;

    private static (string Version, DateTime BuildDate) GetBuildData(Assembly assembly)
    {
        // get version string from custom attribute
        var versionString = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        // split the version string to get the date part
        var versionParts = versionString!.Split("+build");

        var version = versionParts[0];
        var buildDate = DateTime.ParseExact(versionParts[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);

        return (version, buildDate.ToUniversalTime());
    }
}
