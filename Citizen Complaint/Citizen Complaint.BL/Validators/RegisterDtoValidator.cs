using Citizen_Complaint.DAL;
using FluentValidation;

namespace Citizen_Complaint.BL.Validators;
public class RegisterDtoValidator : AbstractValidator<Register>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Firstname is required.")
            .MaximumLength(50).WithMessage("Firstname cannot exceed 50 characters.");
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required.")
            .MaximumLength(50).WithMessage("Lastname cannot exceed 50 characters.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

        RuleFor(x => x.Gender)
            .Must(g => g == 1 || g == 2)
            .WithMessage("Gender must be 1 (Male) or 2 (Female).");
    }
}
