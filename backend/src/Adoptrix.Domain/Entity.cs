namespace Adoptrix.Domain;

public abstract class Entity
{
    public int Id { get; init; }

    public override bool Equals(object? otherObject)
        => otherObject is Entity otherEntity && Id == otherEntity.Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}