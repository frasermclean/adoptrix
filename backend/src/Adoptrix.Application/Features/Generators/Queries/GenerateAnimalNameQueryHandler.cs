using Adoptrix.Application.Features.Generators.Responses;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Generators.Queries;

public class GenerateAnimalNameQueryHandler(IAnimalAssistant animalAssistant)
    : IRequestHandler<GenerateAnimalNameQuery, Result<AnimalNameResponse>>
{
    public async Task<Result<AnimalNameResponse>> Handle(GenerateAnimalNameQuery query,
        CancellationToken cancellationToken)
    {
        return await animalAssistant.GenerateNameAsync(query, cancellationToken);
    }
}
