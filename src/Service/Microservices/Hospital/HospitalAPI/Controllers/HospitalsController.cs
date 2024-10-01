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
        public async Task<IActionResult> GetHospitals(Guid id)
            => await _mediator.Send(new GetHospitalQuery(id));

        [HttpGet("{id}/Rooms")]
        public async Task<IActionResult> GetRooms(Guid id)
            => await _mediator.Send(new GetRoomsQuery(id));
    }
}
