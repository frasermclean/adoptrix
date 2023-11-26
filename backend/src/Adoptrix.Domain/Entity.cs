namespace Adoptrix.Domain;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }

        return other.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}