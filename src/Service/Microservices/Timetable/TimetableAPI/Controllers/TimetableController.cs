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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimetable(int id, UpdateTimetableCommand command)
            => await _mediator.Send(new UpdateTimetableCommand
                (id, 
                command.DoctorId,
                command.HospitalId,
                command.From,
                command.To,
                command.Room));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable(int id)
            => await _mediator.Send(new DeleteTimetableCommand(id));
    }
}
