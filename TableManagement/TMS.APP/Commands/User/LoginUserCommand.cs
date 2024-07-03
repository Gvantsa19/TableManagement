using MediatR;
using TMS.APP.Models;

namespace TMS.APP.Commands.User
{
    public class LoginUserCommand : IRequest<AuthResultDto>
    {
        public LoginUserRequest Model { get; set; }

        public LoginUserCommand(LoginUserRequest model)
        {
            Model = model;
        }
    }
}
