namespace Adoptrix.Core;

public interface ILastModifiedEntity
{
    Guid LastModifiedBy { get; set;  }
    DateTime LastModifiedUtc { get; set; }
}
