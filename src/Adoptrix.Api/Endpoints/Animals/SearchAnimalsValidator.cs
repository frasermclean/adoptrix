using Adoptrix.Contracts.Requests;
using Adoptrix.Core;
using FluentValidation;

namespace Adoptrix.Api.Endpoints.Animals;

public class SearchAnimalsValidator : Validator<SearchAnimalsRequest>
{
    public SearchAnimalsValidator()
    {
        RuleFor(request => request.Sex)
            .Must(sex => sex is null || Enum.TryParse<Sex>(sex, out _))
            .WithMessage("Invalid value");
    }
}
