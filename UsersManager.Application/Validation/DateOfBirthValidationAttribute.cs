using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Validation;

public class DateOfBirthValidationAttribute : ValidationAttribute
{
    private readonly int _maxAge;

    public DateOfBirthValidationAttribute(int maxAge) => _maxAge = maxAge;

    public DateOfBirthValidationAttribute(int maxAge, Func<string> errorMessageAccessor) : base(errorMessageAccessor) =>
        _maxAge = maxAge;

    public DateOfBirthValidationAttribute(int maxAge, string errorMessage) : base(errorMessage) => _maxAge = maxAge;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly dateTime)
            return new ValidationResult("This is not a DateTime.");

        var currentDateTime = DateOnly.FromDateTime(validationContext.GetService<IDateTimeService>()!.Now);

        if (dateTime > currentDateTime.AddYears(-14))
            return new ValidationResult("Date of birth is too small.");

        if (dateTime < currentDateTime.AddYears(-_maxAge))
            return new ValidationResult("Date of birth is too big.");

        return ValidationResult.Success;
    }

    public override bool RequiresValidationContext => true;
}
