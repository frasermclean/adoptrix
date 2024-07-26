namespace Adoptrix.Core;

public class User
{
    public Guid Id { get; init; }
    public UserRole Role { get; init; }
    public DateTime CreatedAt { get; init; }
}
