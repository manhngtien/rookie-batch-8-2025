using AssetManagement.Application.DTOs.ReturningRequests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Validators.ReturningRequests
{
    public class CreateAdminReturningRequestValidator : AbstractValidator<CreateAdminReturningRequest>
    {
        public CreateAdminReturningRequestValidator()
        {
            RuleFor(r => r.AssignmentId)
                .NotEmpty().WithMessage("Assignment ID is required.")
                .GreaterThan(0).WithMessage("Assignment ID must be greater than 0.");
        }
    }
}
