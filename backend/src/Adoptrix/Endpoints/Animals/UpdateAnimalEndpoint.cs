using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Errors;
using Adoptrix.Core.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adoptrix.Endpoints.Animals;

public class UpdateAnimalEndpoint(IAnimalsService animalsService) : Endpoint<UpdateAnimalRequest, AnimalResponse>
{
    public override void Configure()
    {
        Put("animals/{animalId:guid}");
    }

    public override async Task HandleAsync(UpdateAnimalRequest request,
        CancellationToken cancellationToken)
    {
        var result = await animalsService.UpdateAsync(request, cancellationToken);

        if (result.HasError<AnimalNotFoundError>())
        {
            AddError(r => r.AnimalId, "Animal not found");
        }

        if (result.HasError<BreedNotFoundError>())
        {
            AddError(r => r.BreedId, "Breed not found");
        }

        ThrowIfAnyErrors();
        await SendOkAsync(result.Value, cancellationToken);
    }
}
