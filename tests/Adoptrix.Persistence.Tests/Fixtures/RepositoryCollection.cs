using Adoptrix.Persistence.Services;

namespace Adoptrix.Persistence.Tests.Fixtures;

public record RepositoryCollection(
    IAnimalsRepository AnimalsRepository,
    IBreedsRepository BreedsRepository,
    ISpeciesRepository SpeciesRepository);
