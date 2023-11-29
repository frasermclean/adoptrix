using Adoptrix.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Options;
using Sqids;

namespace Adoptrix.Api.Validators;

public sealed class SqidValidator : AbstractValidator<string>
{
    public SqidValidator(IOptions<SqidsOptions> options, ISqidConverter sqidConverter)
    {
        RuleFor(sqid => sqid)
            .MinimumLength(options.Value.MinLength)
            .WithMessage("Must be at least {MinLength} characters long")
            .Must(sqidConverter.IsValid)
            .WithMessage("Invalid format");
    }
}