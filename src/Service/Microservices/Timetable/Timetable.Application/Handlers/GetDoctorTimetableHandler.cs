using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Application.Queries;

namespace Timetable.Application.Handlers
{
    public class GetDoctorTimetableHandler :
        IRequestHandler<GetDoctorTimetableQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;
        private readonly IRequestClient<GetHospitalRequset> _hospitalsClient;
        private readonly IRequestClient<GetUserRequest> _doctorsClient;

        public GetDoctorTimetableHandler(
            IRequestClient<GetUserRequest> doctorsClient,
            IRequestClient<GetHospitalRequset> hospitalsClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
            _hospitalsClient = hospitalsClient;
            _doctorsClient = doctorsClient;
        }


        public async Task<IActionResult> Handle(
            GetDoctorTimetableQuery request, CancellationToken cancellationToken)
        {

            if (request.From == null)
                request.From = DateTimeOffset.MinValue;

            if (request.To == null)
                request.To = DateTimeOffset.MaxValue;


            var timetables = (await _timetableRepository
                .GetAllAsync())
                .Where(timetable => timetable.DoctorId == request.Id &&
                timetable.From >= request.From &&
                timetable.To <= request.To)
                .ToList();
 

            var doctror = (await _doctorsClient
                    .GetResponse<GetUserResponse>(new GetUserRequest(request.Id)))
                    .Message.Data;

            var hospitalTimetable = new Dictionary<string, object>
            {
                { "Доктор:",  $"{doctror.LastName} {doctror.FirstName.First()}" },
            };

            foreach (var hospitalId in timetables
                .Select(timetable => timetable.HospitalId)
                .Distinct())
            {
                var hospetal = (await _hospitalsClient
                    .GetResponse<GetHospitalResponse>(new GetHospitalRequset(hospitalId)))
                    .Message.Data;

                hospitalTimetable[$"Больница: {hospetal.Name}."] = timetables
                    .Where(timetable => timetable.HospitalId == hospitalId)
                    .Select(timetable => $"Приём с {timetable.From} до {timetable.To} кб. {timetable.Room}")
                    .ToList();
            }

            return new OkObjectResult(hospitalTimetable);
        }
    }
}
