using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.GetAnimal;

public class GetAnimalCommandValidator : Validator<GetAnimalCommand>
{
    public GetAnimalCommandValidator(SqidValidator sqidValidator)
    {
        RuleFor(command => command.Id)
            .SetValidator(sqidValidator);
    }
}