namespace Adoptrix.Core;

public interface IUserCreatedEntity
{
    Guid LastModifiedBy { get; }
    DateTime LastModifiedUtc { get; }
}
