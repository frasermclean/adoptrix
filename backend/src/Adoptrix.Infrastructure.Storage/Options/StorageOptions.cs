using System.ComponentModel.DataAnnotations;

namespace Adoptrix.Infrastructure.Storage.Options;

public class StorageOptions
{
    public const string SectionName = "AzureStorage";

    [Required] public string BlobEndpoint { get; init; } = string.Empty;
    [Required] public string QueueEndpoint { get; init; } = string.Empty;
    public BlobContainerNameOptions BlobContainerNames { get; init; } = new();
    public QueueNamesOptions QueueNames { get; init; } = new();

    public class BlobContainerNameOptions
    {
        [Required] public string AnimalImages { get; init; } = string.Empty;
    }

    public class QueueNamesOptions
    {
        [Required] public string AnimalDeleted { get; init; } = string.Empty;
        [Required] public string AnimalImageAdded { get; init; } = string.Empty;
    }
}