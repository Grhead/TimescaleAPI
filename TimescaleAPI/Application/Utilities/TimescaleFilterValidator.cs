using FluentValidation;
using TimescaleAPI.Application.DTOs;

namespace TimescaleAPI.Application.Utilities;

public class TimescaleFilterValidator : AbstractValidator<TimescaleFilterDto>
{
    public TimescaleFilterValidator()
    {
        
    }
}