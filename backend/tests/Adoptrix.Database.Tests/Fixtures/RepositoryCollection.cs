using Adoptrix.Core.Abstractions;

namespace Adoptrix.Database.Tests.Fixtures;

public record RepositoryCollection(
    IAnimalsRepository AnimalsRepository,
    IBreedsRepository BreedsRepository,
    ISpeciesRepository SpeciesRepository);
