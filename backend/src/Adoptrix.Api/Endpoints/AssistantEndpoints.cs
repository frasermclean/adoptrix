using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using MediatR;

namespace Adoptrix.Api.Endpoints;

public static class AssistantEndpoints
{
    public static async Task<AnimalDescriptionResponse> GenerateAnimalDescription(ISender sender,
        [AsParameters] GenerateAnimalDescriptionQuery query)
    {
        var response = await sender.Send(query);
        return response;
    }
}
