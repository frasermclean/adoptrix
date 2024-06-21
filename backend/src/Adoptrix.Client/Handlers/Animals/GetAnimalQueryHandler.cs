using Adoptrix.Domain.Errors;
using Adoptrix.Domain.Models;
using Adoptrix.Domain.Queries.Animals;
using FluentResults;
using MediatR;

namespace Adoptrix.Client.Handlers.Animals;

public class GetAnimalQueryHandler : IRequestHandler<GetAnimalQuery, Result<Animal>>
{
    public async Task<Result<Animal>> Handle(GetAnimalQuery query, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        return new AnimalNotFoundError(query.AnimalId);
    }
}
