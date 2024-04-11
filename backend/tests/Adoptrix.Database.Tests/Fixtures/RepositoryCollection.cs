using Adoptrix.Application.Services;

namespace Adoptrix.Database.Tests.Fixtures;

public class RepositoryCollection
{
    public required IAnimalsRepository AnimalsRepository { get; init; }
    public required IBreedsRepository BreedsRepository { get; init; }
    public required ISpeciesRepository SpeciesRepository { get; init; }
}
