namespace Adoptrix.Core;

public interface ILastModified
{
    Guid? LastModifiedBy { get; set; }
    DateTime LastModifiedUtc { get; set; }
}
