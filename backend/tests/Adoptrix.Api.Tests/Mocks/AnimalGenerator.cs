﻿using Adoptrix.Domain;
using Bogus;

namespace Adoptrix.Api.Tests.Mocks;

public static class AnimalGenerator
{
    private static readonly Faker<Animal> AnimalFaker = new Faker<Animal>()
        .RuleFor(animal => animal.Id, faker => faker.Random.Int(1, 1000))
        .RuleFor(animal => animal.Name, faker => faker.Name.FirstName())
        .RuleFor(animal => animal.Description, faker => faker.Lorem.Paragraph())
        .RuleFor(animal => animal.Species, SpeciesGenerator.Generate);

    public static Animal Generate() => AnimalFaker.Generate();
}