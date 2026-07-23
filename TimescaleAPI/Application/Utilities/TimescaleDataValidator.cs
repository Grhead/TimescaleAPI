using FluentValidation;

namespace TimescaleAPI.Application.Utilities;

public class TimescaleDataValidator : AbstractValidator<TimescaleData>
{
    private readonly DateTime _minDate = new(2000, 1, 1);

    public TimescaleDataValidator()
    {
        RuleFor(timescaleData => timescaleData.Date)
            .NotEmpty().WithMessage("Date cannot be null")
            .InclusiveBetween(_minDate, DateTime.UtcNow)
            .WithMessage("The date cannot be earlier than January 1, 2000, or later than today.");

        RuleFor(timescaleData => timescaleData.ExecutionTime)
            .NotEmpty().WithMessage("Execution time cannot be null")
            .GreaterThan(0).WithMessage("Execution time must be greater than zero");

        RuleFor(timescaleData => timescaleData.Value)
            .NotEmpty().WithMessage("Value cannot be null")
            .GreaterThan(0).WithMessage("Value must be greater than zero");
    }
}