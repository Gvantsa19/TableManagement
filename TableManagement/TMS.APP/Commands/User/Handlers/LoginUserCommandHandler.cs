using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TMS.APP.Models;
using TMS.Infrastructure.Configuration;

namespace TMS.APP.Commands.User.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultDto>
    {
        private readonly UserManager<Infrastructure.Entities.User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IValidator<LoginUserCommand> _validator;

        public LoginUserCommandHandler(UserManager<Infrastructure.Entities.User> userManager, IJwtFactory jwtFactory, IValidator<LoginUserCommand> validator)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _validator = validator;
        }
        public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var model = request.Model;
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid credentials.");
            }

            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>());

            return new AuthResultDto
            {
                Token = token,
                StatusCode = 200
            };
        }
    }
}
