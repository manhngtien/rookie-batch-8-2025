using AssetManagement.Application.DTOs.Assets;
using FluentValidation;

namespace AssetManagement.Application.Validators.Assets;

public class CreateAssetRequestValidator : AbstractValidator<CreateAssetRequest>
{
    public CreateAssetRequestValidator()
    {
        RuleFor(r => r.AssetName)
            .NotEmpty().WithMessage("Asset name is required.");
        
        RuleFor(r => r.CategoryId)
            .NotEmpty().WithMessage("Category is required.");
        
        RuleFor(r => r.Specification)
            .NotEmpty().WithMessage("Specification is required.");
        
        RuleFor(r => r.InstalledDate)
            .NotEmpty().WithMessage("Installed date is required.");
        
        RuleFor(r => r.State)
            .NotEmpty().WithMessage("State is required.");
    }
}