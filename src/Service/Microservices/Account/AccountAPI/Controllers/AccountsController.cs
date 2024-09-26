using Account.Application.Commands;
using Account.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;


        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("Me/{id}")]
        public async Task<IActionResult> Me(Guid id)
            => await _mediator.Send(new MeQuery(id));

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCommand command)
            => await _mediator.Send(command);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAccounts([FromQuery] int from, [FromQuery] int count)
            => await _mediator.Send(new GetAccountsQuery(from, count));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
            => await _mediator.Send(command);

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountCommand command)
            => await _mediator.Send(new UpdateAccountCommand(id,
                command.Roles,
                command.LastName,
                command.FirstName,
                command.Username,
                command.Password));
    }
}
