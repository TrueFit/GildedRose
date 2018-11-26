using FluentValidation;
using GildedRose.Membership.Models;
using System.Linq;

namespace GildedRose.Api.Validators
{
    public class CreateAccountModel_Validator : AbstractValidator<CreateAccountModel>
    {
        public CreateAccountModel_Validator()
        {
            this.RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            this.RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            this.RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
            this.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Must be a vaild email address");
            this.RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be 8 characters or more")
                .MaximumLength(255).WithMessage("Password cannot be more than 255 characters")
                .Must(x => x.Any(char.IsUpper)).WithMessage("Password must have at least one uppercase character")
                .Must(x => x.Any(char.IsLower)).WithMessage("Password must have at least one lowercase character")
                .Must(x => x.Any(char.IsSymbol)).WithMessage("Password must have at least one symbol");
            this.RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords Don't Match");
        }
    }
}
