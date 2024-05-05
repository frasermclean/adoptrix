using Adoptrix.Application.Features.Animals.Responses;
using Adoptrix.Application.Services;
using MediatR;

namespace Adoptrix.Application.Features.Animals.Queries;

public class GenerateAnimalDescriptionQueryHandler(IAnimalAssistant animalAssistant)
: IRequestHandler<GenerateAnimalDescriptionQuery, GenerateAnimalDescriptionResponse>
{
    public async Task<GenerateAnimalDescriptionResponse> Handle(GenerateAnimalDescriptionQuery query, CancellationToken cancellationToken)
    {
        return await animalAssistant.GenerateDescriptionAsync(query, cancellationToken);
    }
}
