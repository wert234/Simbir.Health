using History.Application.Queries;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IMediator = MediatR.IMediator;

namespace HistoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles ="User,Doctor")]
        [HttpGet("Account/{id}")]
        public async Task<IActionResult> GetHistory(int id)
            => await _mediator.Send(new GetHistoryQuerу(id, int.Parse(User.Claims.Last().Value)));
    }
}
