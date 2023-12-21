namespace Adoptrix.Domain;

public class Species : Aggregate
{
    public const int NameMaxLength = 20;

    public static readonly Guid DogSpeciesId = Guid.Parse("f2d44dc6-6c6c-41d0-ad8d-e6c814b09c1a");
    public static readonly Guid CatSpeciesId = Guid.Parse("c9c1836b-1051-45c3-a2c4-0d841e69e6d3");
    public static readonly Guid HorseSpeciesId = Guid.Parse("e6d11a53-bacb-4a8b-a171-beea7e935467");

    public string Name { get; init; } = string.Empty;
}