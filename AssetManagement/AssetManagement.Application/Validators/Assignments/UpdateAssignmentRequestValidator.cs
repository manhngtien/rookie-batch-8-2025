using AssetManagement.Application.DTOs.Assignments;
using FluentValidation;

namespace AssetManagement.Application.Validators.Assignments;

public class UpdateAssignmentRequestValidator : AbstractValidator<UpdateAssignmentRequest>
{
    public UpdateAssignmentRequestValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(r => r.StaffCode)
            .NotEmpty().WithMessage("Staff code is required.");

        RuleFor(r => r.AssetCode)
            .NotEmpty().WithMessage("Asset code is required.");

        RuleFor(r => r.AssignedDate)
            .NotEmpty().WithMessage("AssignedDate is required.");

        RuleFor(r => DateOnly.FromDateTime(r.AssignedDate))
            .NotEmpty().WithMessage("Assigned Date is required.");
    }
}