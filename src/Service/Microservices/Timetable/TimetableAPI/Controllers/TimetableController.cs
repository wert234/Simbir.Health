using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.Application.Commands;
using IMediator = MediatR.IMediator;

namespace TimetableAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    public class TimetableController : ControllerBase
    {
        private readonly IMediator _mediator;


        public TimetableController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> AddTimetable(AddTimetableCommand command)
            => await _mediator.Send(command);
    }
}
