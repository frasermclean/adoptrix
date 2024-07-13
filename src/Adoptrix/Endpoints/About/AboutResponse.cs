﻿namespace Adoptrix.Endpoints.About;

public class AboutResponse
{
    public required string Version { get; init; }
    public required string Environment { get; init; }
    public required DateTime BuildDate { get; init; }
    public required Uri AnimalImagesBaseUrl { get; init; }
}
