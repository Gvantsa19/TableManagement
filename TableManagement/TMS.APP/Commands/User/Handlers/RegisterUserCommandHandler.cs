using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TMS.APP.Models;

namespace TMS.APP.Commands.User.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApplicationResult>
    {
        private readonly UserManager<Infrastructure.Entities.User> _userManager;
        private readonly IValidator<RegisterUserCommand> _validator;
        public RegisterUserCommandHandler(UserManager<Infrastructure.Entities.User> userManager, IValidator<RegisterUserCommand> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new Error(new List<string> { e.ErrorMessage }));
                return new ApplicationResult
                {
                    Success = false,
                    Errors = errors.ToList()
                };
            }

            var model = request.Model;
            var user = new Infrastructure.Entities.User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                NormalizedUserName = model.Email,
                Email = model.Email,
                PasswordHash = model.Password,
                RegistrationDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new ApplicationResult
                {
                    Data = result,
                    Success = true
                };
            }
            else
            {
                var errorMessages = result.Errors.Select(error => error.Description);
                var error = new Error(errorMessages);

                return new ApplicationResult
                {
                    Success = false,
                    Errors = new List<Error> { error }
                };
            }
        }
    }
}
