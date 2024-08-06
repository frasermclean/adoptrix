namespace Adoptrix.Core;

public interface IUserCreatedEntity
{
    Guid CreatedBy { get; }
    DateTime CreatedAt { get; }
}
