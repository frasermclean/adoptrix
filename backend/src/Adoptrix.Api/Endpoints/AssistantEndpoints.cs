using Adoptrix.Api.Extensions;
using Adoptrix.Application.Features.Animals.Queries;
using Adoptrix.Application.Features.Animals.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Api.Endpoints;

public static class AssistantEndpoints
{
    public static IEndpointRouteBuilder MapAssistantsEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/assistants");
        group.MapGet("animal-description", GenerateAnimalDescription);

        return builder;
    }

    private static async Task<Results<Ok<AnimalDescriptionResponse>, ProblemHttpResult>> GenerateAnimalDescription(
        ISender sender, [AsParameters] GenerateAnimalDescriptionQuery query)
    {
        var result = await sender.Send(query);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : TypedResults.Problem(result.Errors.ToProblemDetails());
    }
}
