namespace Adoptrix.Domain;

public abstract class Aggregate : Entity
{
    /// <summary>
    /// User Id of the user who created the <see cref="Aggregate"/>
    /// </summary>
    public Guid CreatedBy { get; init; }

    /// <summary>
    /// Date and time the <see cref="Aggregate"/> was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
