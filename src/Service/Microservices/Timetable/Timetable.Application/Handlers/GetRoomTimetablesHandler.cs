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
    public class GetRoomTimetablesHandler : IRequestHandler<GetRoomTimetablesQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;
        private readonly IRequestClient<GetHospitalRequset> _hospitalsClient;
        private readonly IRequestClient<GetUserRequest> _doctorsClient;

        public GetRoomTimetablesHandler(
            IRequestClient<GetUserRequest> doctorsClient,
            IRequestClient<GetHospitalRequset> hospitalsClient,
            IRepository<Domain.Entitys.Timetable, int> timetableRepository)
        {
            _timetableRepository = timetableRepository;
            _hospitalsClient = hospitalsClient;
            _doctorsClient = doctorsClient;
        }


        public async Task<IActionResult> Handle(
            GetRoomTimetablesQuery request, CancellationToken cancellationToken)
        {

            if (request.From == null)
                request.From = DateTimeOffset.MinValue;

            if (request.To == null)
                request.To = DateTimeOffset.MaxValue;


            var timetables = (await _timetableRepository
                .GetAllAsync())
                .Where(timetable => timetable.HospitalId == request.Id &&
                timetable.Room == request.Room &&
                timetable.From >= request.From &&
                timetable.To <= request.To)
                .ToList();

            var hospetal = await _hospitalsClient.GetResponse<GetHospitalResponse>(new GetHospitalRequset(request.Id));

            var hospitalTimetable = new Dictionary<string, object>
            {
                { "Больница:", hospetal.Message.Data.Name },
            };

            hospitalTimetable[$"Кабинет: {request.Room}."] = timetables
                .Select(timetable => $"Приём с {timetable.From} до {timetable.To}")
                .ToList();

            return new OkObjectResult(hospitalTimetable);
        }
    }
}
