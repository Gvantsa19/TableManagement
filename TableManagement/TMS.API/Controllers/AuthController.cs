using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using MediatR;
using TMS.APP.Models;
using TMS.APP.Commands.User;

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterUserRequest), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(APP.Models.Error), (int)HttpStatusCode.BadRequest)]
        public async Task<ApplicationResult> Register([FromBody] RegisterUserRequest model)
        {
            return await _mediator.Send(new RegisterUserCommand(model));
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResultDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(APP.Models.Error), (int)HttpStatusCode.BadRequest)]
        public async Task<AuthResultDto> Login([FromBody] LoginUserRequest model)
        {
            return await _mediator.Send(new LoginUserCommand(model));
        }
    }
}
