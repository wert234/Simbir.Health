using Hospital.Application.Commands;
using Hospital.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HospitalsController : ControllerBase
    {
        private readonly IMediator _mediator;


        public HospitalsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{from}/{count}")]
        public async Task<IActionResult> GetHospitals(int from, int count)
            => await _mediator.Send(new GetHospitalsQuery(from, count));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHospitals(int id)
            => await _mediator.Send(new GetHospitalQuery(id));

        [HttpGet("{id}/Rooms")]
        public async Task<IActionResult> GetRooms(int id)
            => await _mediator.Send(new GetRoomsQuery(id));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddHospital(AddHospitalCommand command)
            => await _mediator.Send(command);

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHospital(int id, UpdateHospitalCommand command)
            => await _mediator.Send(new UpdateHospitalCommand(
                id,
                command.Name,
                command.Address,
                command.ContactPhone,
                command.Rooms));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHospital(int id)
            => await _mediator.Send(new DeleteHospitalCommand(id));
    }
}
