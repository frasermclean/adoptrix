using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using FluentResults;

namespace Adoptrix.Application.Services;

public interface IBreedsService
{
    Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public class BreedsService(IBreedsRepository repository) : IBreedsService
{
    public async Task<Result<Breed>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await repository.GetByNameAsync(name, cancellationToken);
    }
}
