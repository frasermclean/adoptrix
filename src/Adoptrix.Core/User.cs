namespace Adoptrix.Core;

public class User
{
    public Guid Id { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? EmailAddress { get; set; }
    public UserRole Role { get; init; }
    public DateTime CreatedAt { get; init; }
}
