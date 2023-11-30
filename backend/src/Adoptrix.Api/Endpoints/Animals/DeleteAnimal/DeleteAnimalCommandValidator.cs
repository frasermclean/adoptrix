using Adoptrix.Api.Validators;
using Adoptrix.Application.Commands.Animals;
using FastEndpoints;

namespace Adoptrix.Api.Endpoints.Animals.DeleteAnimal;

public class DeleteAnimalCommandValidator
    : Validator<DeleteAnimalCommand>
{
    public DeleteAnimalCommandValidator(SqidValidator sqidValidator)
    {
        RuleFor(command => command.Id)
            .SetValidator(sqidValidator);
    }
}