using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using Sherad.Domain.DTOs;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Timetable.Application.Queries;
using Timetable.Domain.Entitys;

namespace Timetable.Application.Handlers
{
    public class GetHospitalTimetableHandler :
        IRequestHandler<GetHospitalTimetableQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;
        private readonly IRequestClient<GetHospitalRequset> _hospitalsClient;
        private readonly IRequestClient<GetUserRequest> _doctorsClient;

        public GetHospitalTimetableHandler(
            IRequestClient<GetUserRequest> doctorsClient,
            IRequestClient<GetHospitalRequset> hospitalsClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
            _hospitalsClient = hospitalsClient;
            _doctorsClient = doctorsClient;
        }


        public async Task<IActionResult> Handle(
            GetHospitalTimetableQuery request, CancellationToken cancellationToken)
        {

            if(request.From == null)
                request.From = DateTimeOffset.MinValue;
            
            if (request.To == null)
                request.To = DateTimeOffset.MaxValue;
            

            var timetables = (await _timetableRepository
                .GetAllAsync())
                .Where(timetable => timetable.HospitalId == request.Id &&
                timetable.From >= request.From &&
                timetable.To <= request.To)
                .ToList();

            var hospetal = await _hospitalsClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(request.Id));

            var hospitalTimetable = new Dictionary<string, object>
            {
                { "Больница:", hospetal.Message.Data.Name },
            };

            foreach(var doctorId in timetables
                .Select(timetable => timetable.DoctorId)
                .Distinct())
            {
                var doctror = (await _doctorsClient
                    .GetResponse<GetUserResponse>(new GetUserRequest(doctorId)))
                    .Message.Data;

                hospitalTimetable[$"Доктор: {doctror.LastName} {doctror.FirstName.First()}."] = timetables
                    .Where(timetable => timetable.DoctorId == doctorId)
                    .Select(timetable => $"Приём с {timetable.From} до {timetable.To} кб. {timetable.Room}")
                    .ToList();
            }

            return new OkObjectResult(hospitalTimetable);
        }
    }
}
