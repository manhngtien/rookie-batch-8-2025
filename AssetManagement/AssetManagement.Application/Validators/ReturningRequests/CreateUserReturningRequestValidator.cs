using AssetManagement.Application.DTOs.ReturningRequests;
using FluentValidation;

namespace AssetManagement.Application.Validators.Categories;

public class CreateUserReturningRequestValidator : AbstractValidator<CreateUserReturningRequest>
{
    public CreateUserReturningRequestValidator()
    {
        RuleFor(r => r.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.");
    }
}