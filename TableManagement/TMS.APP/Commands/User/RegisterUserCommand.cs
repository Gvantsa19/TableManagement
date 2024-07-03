using MediatR;
using TMS.APP.Models;

namespace TMS.APP.Commands.User
{
    public class RegisterUserCommand : IRequest<ApplicationResult>
    {
        public RegisterUserRequest Model { get; set; }

        public RegisterUserCommand(RegisterUserRequest model)
        {
            Model = model;
        }
    }
}
