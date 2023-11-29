using Adoptrix.Api.Validators;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.AddAnimalImages;

public class AddAnimalImagesRequestValidator : Validator<AddAnimalImagesRequest>
{
    public AddAnimalImagesRequestValidator(SqidValidator sqidValidator)
    {
        RuleFor(request => request.Id)
            .SetValidator(sqidValidator);
    }
}