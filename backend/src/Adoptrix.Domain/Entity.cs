namespace Adoptrix.Domain;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public override bool Equals(object? otherObject)
        => otherObject is Entity otherEntity && Id == otherEntity.Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}