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
    public class AddAppointmentsHandler : IRequestHandler<AddAppointmentsCommand, IActionResult>
    {
        private IRepository<Appointment, int> _repository;

        public AddAppointmentsHandler(IRepository<Appointment, int> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Handle(AddAppointmentsCommand request, CancellationToken cancellationToken)
        {
            await _repository.AddAsync(new Appointment
            {
                TimetableId = request.Id,
                Date = request.Time,          
            });

            return new NoContentResult();
        }
    }
}
