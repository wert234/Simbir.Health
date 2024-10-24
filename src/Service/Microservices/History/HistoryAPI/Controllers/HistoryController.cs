using History.Application.Queries;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IMediator = MediatR.IMediator;
using History.Application.Commands;

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

        [Authorize(Roles = "User,Doctor")]
        [HttpGet("Account/{id}")]
        public async Task<IActionResult> GetHistory(int id)
            => await _mediator.Send(new GetHistoryQuery(id, int.Parse(User.Claims.Last().Value)));

        [Authorize(Roles = "User,Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailHistory(int id)
            => await _mediator.Send(new GetDetailHistoryQuery(id, int.Parse(User.Claims.Last().Value)));

        [Authorize(Roles = "Admin,Manager,Doctor")]
        [HttpPost]
        public async Task<IActionResult> AddHistory(AddHistoryCommand command)
            => await _mediator.Send(command);

        [Authorize(Roles = "Admin,Manager,Doctor")]
        [HttpPut("{id}")]
                public async Task<IActionResult> UpdateHistory(UpdateHistoryCommand command, int id)
            => await _mediator.Send(new UpdateHistoryCommand
            {
                Id = id,
                Data = command.Data,
                Date = command.Date,
                DoctorId = command.DoctorId,
                HospitalId = command.HospitalId,
                PacientId = command.PacientId,
                Room = command.Room,
            });
    }
}
