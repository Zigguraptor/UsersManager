using System.ComponentModel.DataAnnotations;

namespace UsersManager.Application.Validation;

public class DateTimeBetweenAttribute : ValidationAttribute
{
    private readonly DateTime _from;
    private readonly DateTime _to;

    public DateTimeBetweenAttribute(DateTime from, DateTime to)
    {
        if (from > to)
            throw new ArgumentException("\"from\" is greater then \"to\"");

        _from = from;
        _to = to;
    }

    public DateTimeBetweenAttribute(DateTime from, DateTime to, string errorMessage) : base(errorMessage)
    {
        if (from > to)
            throw new ArgumentException("\"from\" is greater then \"to\"");

        _from = from;
        _to = to;
    }

    public DateTimeBetweenAttribute(DateTime from, DateTime to, Func<string> errorMessageAccessor) : base(
        errorMessageAccessor)
    {
        if (from > to)
            throw new ArgumentException("\"from\" is greater then \"to\"");

        _from = from;
        _to = to;
    }

    public override bool RequiresValidationContext => true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime dateTime)
            return new ValidationResult("This is not a DateTime");

        if (dateTime < _from || dateTime >= _to)
            return new ValidationResult($"This DateTime {dateTime} is not between {_from} and {_to}");

        return ValidationResult.Success;
    }
}
