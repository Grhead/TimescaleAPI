using FluentValidation;
using TimescaleAPI.Application.DTOs;

namespace TimescaleAPI.Application.Utilities;

public class TimescaleDataValidator : AbstractValidator<TimescaleValueDto>
{
    private readonly DateTime _minDate = new(2000, 1, 1);

    public TimescaleDataValidator()
    {
        RuleFor(timescaleData => timescaleData.Date)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Date cannot be null")
            .Must(date => date.Value.ToUniversalTime() >= _minDate && date.Value.ToUniversalTime() <= DateTime.UtcNow)
            .WithMessage("The date cannot be earlier than January 1, 2000, or later than today.");

        RuleFor(timescaleData => timescaleData.ExecutionTime)
            .NotNull().WithMessage("Execution time cannot be null")
            .GreaterThan(0).WithMessage("Execution time must be greater than zero");

        RuleFor(timescaleData => timescaleData.Value)
            .NotNull().WithMessage("Value cannot be null")
            .GreaterThan(0).WithMessage("Value must be greater than zero");
    }
}