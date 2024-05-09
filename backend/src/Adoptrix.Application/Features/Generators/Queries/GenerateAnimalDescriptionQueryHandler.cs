using Adoptrix.Application.Features.Generators.Responses;
using Adoptrix.Application.Services;
using FluentResults;
using MediatR;

namespace Adoptrix.Application.Features.Generators.Queries;

public class GenerateAnimalDescriptionQueryHandler(IAnimalAssistant animalAssistant)
    : IRequestHandler<GenerateAnimalDescriptionQuery, Result<AnimalDescriptionResponse>>
{
    public async Task<Result<AnimalDescriptionResponse>> Handle(GenerateAnimalDescriptionQuery query,
        CancellationToken cancellationToken)
    {
        return await animalAssistant.GenerateDescriptionAsync(query, cancellationToken);
    }
}
