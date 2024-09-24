using Account.Application.Commands;
using Account.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [HttpPut("SignOut")]
        public async Task<IActionResult> SignOut(SignOutCommand command)
            => await _mediator.Send(command);

        [HttpGet("Validate")]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(string accessToken)
             => await _mediator.Send(new ValidateQuery(accessToken));

        [HttpPost("Refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(string refreshToken)
            => await _mediator.Send(new RefreshCommand(refreshToken));
    }
}
