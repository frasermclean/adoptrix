using Adoptrix.Application.Services;

namespace Adoptrix.Database.Tests.Fixtures;

public record RepositoryCollection(
    IAnimalsRepository AnimalsRepository,
    IBreedsRepository BreedsRepository,
    ISpeciesRepository SpeciesRepository);
