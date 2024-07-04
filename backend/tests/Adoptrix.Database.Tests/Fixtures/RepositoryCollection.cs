using Adoptrix.Application.Services.Abstractions;

namespace Adoptrix.Database.Tests.Fixtures;

public record RepositoryCollection(
    IAnimalsRepository AnimalsRepository,
    IBreedsRepository BreedsRepository,
    ISpeciesRepository SpeciesRepository);
