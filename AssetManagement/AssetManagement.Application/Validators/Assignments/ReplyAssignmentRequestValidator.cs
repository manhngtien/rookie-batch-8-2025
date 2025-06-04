using AssetManagement.Application.DTOs.Assignments;
using FluentValidation;

namespace AssetManagement.Application.Validators.Assignments;

public class ReplyAssignmentRequestValidator : AbstractValidator<ReplyAssignmentRequest>
{
    public ReplyAssignmentRequestValidator()
    {
        RuleFor(r => r.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");

        RuleFor(r => r.IsAccepted)
            .NotNull().WithMessage("Action is required.");
    }
}