﻿using Adoptrix.Domain;

namespace Adoptrix.Application.Models;

public class AnimalSearchResult(Animal animal)
{
    public Guid Id => animal.Id;
    public string Name => animal.Name;
    public string? Description => animal.Description;
    public Species Species => animal.Species;
}