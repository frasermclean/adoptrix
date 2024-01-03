using Adoptrix.Api.Tests.Generators;
using Adoptrix.Application.Services.Repositories;
using Adoptrix.Domain;
using Adoptrix.Domain.Errors;
using FluentResults;

namespace Adoptrix.Api.Tests.Mocks;

public class MockSpeciesRepository : ISpeciesRepository
{
    public const string UnknownSpeciesName = "unknown";

    public Task<IEnumerable<Species>> SearchSpeciesAsync(CancellationToken cancellationToken = default)
    {
        var species = SpeciesGenerator.Generate(3);
        return Task.FromResult(species);
    }

    public Task<Result<Species>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (name == UnknownSpeciesName)
        {
            var error = new SpeciesNotFoundError(UnknownSpeciesName);
            var result = new Result<Species>().WithError(error);

            return Task.FromResult(result);
        }

        var species = SpeciesGenerator.Generate();
        return Task.FromResult(Result.Ok(species));
    }
}