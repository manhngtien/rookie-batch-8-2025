using AssetManagement.Application.DTOs.Assignments;
using FluentValidation;

namespace AssetManagement.Application.Validators.Assignments;

public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
{
    public CreateAssignmentRequestValidator()
    {
        RuleFor(r => r.StaffCode)
            .NotEmpty().WithMessage("Staff code is required.");

        RuleFor(r => r.AssetCode)
            .NotEmpty().WithMessage("Asset code is required.");

        RuleFor(r => r.AssignedDate)
            .NotEmpty().WithMessage("AssignedDate is required.");

        RuleFor(r => DateOnly.FromDateTime(r.AssignedDate))
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Assigned Date must be current or future date.");
    }
}