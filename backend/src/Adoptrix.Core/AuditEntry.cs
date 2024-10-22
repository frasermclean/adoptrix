namespace Adoptrix.Core;

public class AuditEntry
{
    public const int OperationNameMaxLength = 40;
    public const int ErrorMessageMaxLength = 512;
    public const int MetadataMaxLength = 2048;

    public int Id { get; init; }
    public Guid UserId { get; init; }
    public required string OperationName { get; init; }
    public DateTime StartTimeUtc { get; init; } = DateTime.UtcNow;
    public DateTime EndTimeUtc { get; set; }
    public bool WasSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Metadata { get; set; }
}
