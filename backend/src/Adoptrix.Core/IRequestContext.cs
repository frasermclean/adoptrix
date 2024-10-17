namespace Adoptrix.Core;

public interface IRequestContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
}
