using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Timetable.Application.Commands;
using Timetable.Application.Queries;
using IMediator = MediatR.IMediator;

namespace TimetableAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly IMediator _mediator;


        public TimetableController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> AddTimetable(AddTimetableCommand command)
            => await _mediator.Send(command);

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimetable(int id,
            UpdateTimetableCommand command)
            => await _mediator.Send(new UpdateTimetableCommand
                (id, 
                command.DoctorId,
                command.HospitalId,
                command.From,
                command.To,
                command.Room));

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable(int id)
            => await _mediator.Send(new DeleteTimetableCommand(id));

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("Doctor/{id}")]
        public async Task<IActionResult> DeleteDoctorTimetables(int id)
            => await _mediator.Send(new DeleteDoctorTimetablesCommand(id));

        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("Hospital/{id}")]
        public async Task<IActionResult> DeleteHospitalTimetables(int id)
            => await _mediator.Send(new DeleteHospitalTimetablesCommand(id));

        [Authorize]
        [HttpGet("Hospital/{id}")]
        public async Task<IActionResult> GetHospitalTimetables(int id,
            DateTimeOffset? from, DateTimeOffset? to)
            => await _mediator.Send(new GetHospitalTimetableQuery(id, from, to));

        [Authorize]
        [HttpGet("Doctor/{id}")]
        public async Task<IActionResult> GetDoctorTimetables(int id,
            DateTimeOffset? from, DateTimeOffset? to)
            => await _mediator.Send(new GetDoctorTimetableQuery(id, from, to));

        [Authorize(Roles = "Admin,Manager,Doctor")]
        [HttpGet("Hospital/{id}/Room/{room}")]
        public async Task<IActionResult> GetRoomTimetables(int id, string room,
            DateTimeOffset? from, DateTimeOffset? to)
            => await _mediator.Send(new GetRoomTimetablesQuery(id, room, from, to));

        [Authorize]
        [HttpGet("{id}/Appointments")]
        public async Task<IActionResult> GetAppointments(int id)
            => await _mediator.Send(new GetAppointmentsQuery(id));
    }
}
