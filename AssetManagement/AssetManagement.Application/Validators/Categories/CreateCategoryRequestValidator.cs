using AssetManagement.Application.DTOs.Categories;
using FluentValidation;

namespace AssetManagement.Application.Validators.Categories;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.CategoryName)
            .NotEmpty().WithMessage("Category name is required.");
        
        RuleFor(r => r.Prefix)
            .NotEmpty().WithMessage("Prefix is required.");
    }
}