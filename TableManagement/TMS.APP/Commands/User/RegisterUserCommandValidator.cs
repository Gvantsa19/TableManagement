using FluentValidation;

namespace TMS.APP.Commands.User
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Model.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.");
            RuleFor(x => x.Model.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");
            RuleFor(x => x.Model.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required.");
            RuleFor(x => x.Model.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }

    }
}
