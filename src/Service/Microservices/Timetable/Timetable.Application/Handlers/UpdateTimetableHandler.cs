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
    public class UpdateTimetableHandler : IRequestHandler<UpdateTimetableCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;


        public UpdateTimetableHandler(IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }


        public async Task<IActionResult> Handle(UpdateTimetableCommand request, CancellationToken cancellationToken)
        {
            var timetable = await _timetableRepository.GetAsync(request.Id);

            timetable.HospitalId = request.HospitalId;
            timetable.DoctorId = request.DoctorId;
            timetable.From = request.From;
            timetable.To = request.To;
            timetable.Room = request.Room;

            await _timetableRepository.UpdateAsync(timetable);

            return new NoContentResult();
        }
    }
}
