using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;
using Timetable.Domain.Entitys;

namespace Timetable.Application.Handlers
{
    public class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, IActionResult>
    {
        private readonly IRepository<Appointment, int> _repository;

        public DeleteAppointmentHandler(IRepository<Appointment, int> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.AppointmentId);

            return new NoContentResult();
        }
    }
}
