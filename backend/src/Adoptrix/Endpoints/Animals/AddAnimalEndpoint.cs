using Adoptrix.Core.Contracts.Requests.Animals;
using Adoptrix.Core.Contracts.Responses;
using Adoptrix.Core.Errors;
using Adoptrix.Core.Services;
using FastEndpoints;

namespace Adoptrix.Endpoints.Animals;

public class AddAnimalEndpoint(IAnimalsService animalsService) : Endpoint<AddAnimalRequest, AnimalResponse>
{
    public override void Configure()
    {
        Post("animals");
    }

    public override async Task HandleAsync(AddAnimalRequest request, CancellationToken cancellationToken)
    {
        var result = await animalsService.AddAsync(request, cancellationToken);

        if (result.HasError<BreedNotFoundError>())
        {
            AddError(r => r.BreedId, "Breed not found");
        }

        ThrowIfAnyErrors();

        await SendCreatedAtAsync<GetAnimalEndpoint>(new { AnimalId = result.Value.Id }, result.Value,
            cancellation: cancellationToken);
    }
}
