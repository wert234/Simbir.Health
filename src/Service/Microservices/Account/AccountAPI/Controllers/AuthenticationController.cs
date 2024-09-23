using Account.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;


        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpCommand command)
            => await _mediator.Send(command);

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInCommand command)
            => await _mediator.Send(command);
    }
}
