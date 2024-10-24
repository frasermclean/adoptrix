namespace Adoptrix.Core;

public interface IModifiable
{
    Guid? LastModifiedBy { get; set; }
    DateTime LastModifiedUtc { get; set; }
}
