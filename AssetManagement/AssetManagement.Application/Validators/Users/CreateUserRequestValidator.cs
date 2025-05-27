using AssetManagement.Application.DTOs.Accounts;
using AssetManagement.Application.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Validators.Users
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .Must(dob => DateTime.Today.AddYears(-18) >= dob)
                .WithMessage("User is under 18. Please select a different date");

            RuleFor(x => x.JoinedDate)
                .NotEmpty().WithMessage("Joined Date is required")
                .GreaterThan(x => x.DateOfBirth)
                .WithMessage("Joined date is not later than Date of Birth. Please select a different date")
                .Must(joinedDate => joinedDate.DayOfWeek != DayOfWeek.Saturday && joinedDate.DayOfWeek != DayOfWeek.Sunday)
                .WithMessage("Joined date is Saturday or Sunday. Please select a different date");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid role type");
        }
    }
}
