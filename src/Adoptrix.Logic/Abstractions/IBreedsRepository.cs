using Adoptrix.Core;

namespace Adoptrix.Logic.Abstractions;

public interface IBreedsRepository
{
    Task<Breed?> GetAsync(int breedId, CancellationToken cancellationToken = default);
}
