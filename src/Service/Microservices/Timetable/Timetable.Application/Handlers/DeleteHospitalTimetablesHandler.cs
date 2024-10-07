﻿using MediatR;
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
    public class DeleteHospitalTimetablesHandler :
        IRequestHandler<DeleteHospitalTimetablesCommand, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;


        public DeleteHospitalTimetablesHandler(IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }


        public async Task<IActionResult> Handle(DeleteHospitalTimetablesCommand request, CancellationToken cancellationToken)
        {
            var timeteables = (await _timetableRepository
                .GetAllAsync())
                .Where(doctor => doctor.HospitalId == request.Id);

            foreach (var timeteable in timeteables)
            {
                await _timetableRepository.DeleteAsync(timeteable.Id);
            }

            return new NoContentResult();
        }
    }
}