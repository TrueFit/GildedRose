using FluentValidation;
using GildedRose.Membership.Models;

namespace GildedRose.Api.Validators
{
    public class CreateAccount_Validator : AbstractValidator<CreateAccountModel>
    {
        public CreateAccount_Validator()
        {
            this.RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            this.RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
            this.RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
            this.RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            this.RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            this.RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords Don't Match");
        }
    }
}
