﻿using AssetManagement.Application.DTOs.Accounts;
using AssetManagement.Application.DTOs.Users;
using AssetManagement.Core.Enums;
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
                .NotEmpty().WithMessage("Type is required")
                .Must(BeValidRoleType).WithMessage("Invalid role type. Valid values are: Admin, Staff");

            When(x => x.Type?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true, () =>
            {
                RuleFor(x => x.Location)
                    .NotEmpty().WithMessage("Location is required for Admin users")
                    .Must(BeValidLocationType).WithMessage("Invalid location. Valid values are: HCM, HN, DN");
            });
        }

        private bool BeValidRoleType(string type)
        {
            return Enum.TryParse<ERole>(type, true, out _);
        }

        private bool BeValidLocationType(string location)
        {
            return Enum.TryParse<ELocation>(location, true, out _);
        }
    }
}