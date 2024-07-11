using Adoptrix.Core.Abstractions;

namespace Adoptrix.Persistence.Tests.Fixtures;

public record RepositoryCollection(
    IAnimalsRepository AnimalsRepository,
    IBreedsRepository BreedsRepository,
    ISpeciesRepository SpeciesRepository);
