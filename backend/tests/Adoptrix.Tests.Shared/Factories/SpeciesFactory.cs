﻿using Adoptrix.Core;

namespace Adoptrix.Tests.Shared.Factories;

public static class SpeciesFactory
{
    private static readonly string[] Names = ["Dog", "Cat", "Rabbit"];

    public static Species Create(string? name = null) => new()
    {
        Name = name ?? Names[Random.Shared.Next(Names.Length)]
    };

    public static IEnumerable<Species> CreateMany(int count) => Enumerable.Range(0, count).Select(_ => Create());
}
