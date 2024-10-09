using Adoptrix.Core;

namespace Adoptrix.Api.Endpoints.Breeds;

public class BreedResponseMapper : ResponseMapper<BreedResponse, Breed>
{
    public override BreedResponse FromEntity(Breed breed) => new()
    {
        Id = breed.Id,
        Name = breed.Name,
        SpeciesName = breed.Species.Name,
        AnimalCount = breed.Animals.Count
    };
}
