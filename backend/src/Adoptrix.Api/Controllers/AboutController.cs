using System.Globalization;
using System.Reflection;
using Adoptrix.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adoptrix.Api.Controllers;

public class AboutController : ApiController
{
    private static readonly AboutResponse AboutResponse;

    static AboutController()
    {
        var buildData = GetBuildData();

        AboutResponse = new AboutResponse(
            Version: buildData.Version,
            Environment: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            BuildDate: buildData.BuildDate);
    }

    [HttpGet, AllowAnonymous]
    public AboutResponse GetAbout() => AboutResponse;

    private static (string Version, DateTime BuildDate) GetBuildData()
    {
        // get version string from custom attribute
        var versionString = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            !.InformationalVersion;

        // split the version string to get the date part
        var versionParts = versionString.Split("+build");

        var version = versionParts[0];
        var buildDate = DateTime.ParseExact(versionParts[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);

        return (version, buildDate.ToUniversalTime());
    }
}
