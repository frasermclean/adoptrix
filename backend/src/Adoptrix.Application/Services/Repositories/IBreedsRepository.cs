using Adoptrix.Application.Models;

namespace Adoptrix.Application.Services.Repositories;

public interface IBreedsRepository
{
    Task<IEnumerable<SearchBreedsResult>> SearchBreedsAsync(CancellationToken cancellationToken = default);
}