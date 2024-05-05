using System.ComponentModel.DataAnnotations;

namespace Adoptrix.AI.Options;

public class OpenAiOptions
{
    public const string SectionName = "OpenAi";

    [Required] public string Endpoint { get; init; } = string.Empty;
    [Required] public string DeploymentName { get; init; } = string.Empty;
}
