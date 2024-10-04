using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Commands;

namespace Timetable.Application.Handlers
{
    public class AddTimetableHandler : IRequestHandler<AddTimetableCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;


        public AddTimetableHandler(IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }


        public async Task<IActionResult> Handle(AddTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetable = new Domain.Entitys.Timetable
            {
                HospitalId = request.HospitalId,
                DoctorId = request.DoctorId,
                From = request.From,
                To = request.To,
                Room = request.Room
            };

            await _timetableRepository.AddAsync(timetable);

            return new NoContentResult();
        }
    }
}
