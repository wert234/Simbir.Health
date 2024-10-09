using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sherad.Application.Repositories;
using Timetable.Application.Queries;
using Timetable.Domain.Entitys;

namespace Timetable.Application.Handlers
{
    public class GetAppointmentsHandler : IRequestHandler<GetAppointmentsQuery, IActionResult>
    {
        private readonly IRepository<Domain.Entitys.Timetable, int> _timetableRepository;
        private readonly IRepository<Appointment, int> _appointmentRepository;


        public GetAppointmentsHandler(
            IRepository<Domain.Entitys.Timetable, int> timetableRepository
            , IRepository<Appointment, int> appointmentRepository)
        {
            _timetableRepository = timetableRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IActionResult> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var timetable = await _timetableRepository.GetAsync(request.Id);
            var result = new List<Appointment>();

            var appointments = (await _appointmentRepository
                .GetAllAsync())
                .Where(appointment => appointment.TimetableId == request.Id);

            for (int minutes = 30; timetable.From < timetable.To;)
            {

                if (!appointments.Any(appointment => appointment.Date == timetable.From))
                {
                    result.Add(new Appointment
                    {
                        TimetableId = timetable.Id,
                        Date = timetable.From,
                    });
                }

                timetable.From = timetable.From.AddMinutes(minutes);
            }

            return new OkObjectResult(result);
        }
    }
}
