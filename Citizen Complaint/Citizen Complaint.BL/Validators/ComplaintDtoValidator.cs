using FluentValidation;
namespace Citizen_Complaint.BL.Validators;

public class ComplaintDtoValidator : AbstractValidator<ComplaintDto>
{
    public ComplaintDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(99).WithMessage("Name cannot exceed 99 characters.");
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(c => c.Nationalid)
            .NotEmpty().WithMessage("National ID is required.")
            .Length(14).WithMessage("National ID must be exactly 14 characters long.");
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(99).WithMessage("Description cannot exceed 99 characters.");
        RuleFor(c => c.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(99).WithMessage("Location cannot exceed 99 characters.");
    }

}
