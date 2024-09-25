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
    }
}
