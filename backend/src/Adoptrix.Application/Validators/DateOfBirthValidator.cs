using FluentValidation;

namespace Adoptrix.Application.Validators;

public class DateOfBirthValidator : AbstractValidator<DateOnly>
{
    public DateOfBirthValidator()
    {
        RuleFor(dob => dob)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past");
    }
}
