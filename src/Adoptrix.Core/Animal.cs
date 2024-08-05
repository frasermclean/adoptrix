﻿namespace Adoptrix.Core;

public class Animal : IUserCreatedEntity
{
    public const int NameMaxLength = 30;
    public const int DescriptionMaxLength = 2000;
    public const int SlugMaxLength = 60;

    public int Id { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required Breed Breed { get; set; }
    public required Sex Sex { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Slug { get; init; }
    public List<AnimalImage> Images { get; init; } = [];
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? otherObject)
        => otherObject is Animal otherAnimal && Id == otherAnimal.Id;

    public override int GetHashCode()
        => Id.GetHashCode();
}
