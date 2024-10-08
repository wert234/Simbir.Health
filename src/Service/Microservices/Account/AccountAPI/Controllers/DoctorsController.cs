﻿using Account.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator _mediator;


        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{from}/{count}")]
        public async Task<IActionResult> Get(string? nameFilter, int from, int count)
            => await _mediator.Send(new DoctorsQuery(nameFilter, from, count));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => await _mediator.Send(new DoctorQuery(id));
    }
}
