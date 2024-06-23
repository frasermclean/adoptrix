using Adoptrix.Application.Extensions;
using Adoptrix.Application.Mapping;
using Adoptrix.Application.Services;
using Adoptrix.Domain.Models.Responses;
using Adoptrix.Domain.Queries.Animals;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class GetAnimalEndpoint(IAnimalsRepository animalsRepository, IAnimalImageManager animalImageManager)
    : EndpointWithoutRequest<Results<Ok<AnimalResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("animals/{animalId}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<AnimalResponse>, NotFound>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var animalId = Route<Guid>("animalId");
        var animal = await animalsRepository.GetByIdAsync(animalId, cancellationToken);
        if (animal is null)
        {
            return TypedResults.NotFound();
        }

        var response = animal.ToResponse();

        // generate image urls
        foreach (var imageResponse in response.Images)
        {
            if (!imageResponse.IsProcessed)
            {
                continue;
            }

            imageResponse.SetImageUrls(animalId, animalImageManager.ContainerUri);
        }

        return TypedResults.Ok(response);
    }
}
