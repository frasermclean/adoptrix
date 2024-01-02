using System.ComponentModel.DataAnnotations;

namespace Adoptrix.Infrastructure.Storage.Options;

public class StorageOptions
{
    public const string SectionName = "AzureStorage";

    [Required] public string BlobEndpoint { get; init; } = string.Empty;
    [Required] public string QueueEndpoint { get; init; } = string.Empty;
}