using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Commands
{
    public class UpdateTimetableCommand : IRequest<IActionResult>
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Room { get; set; }


        public UpdateTimetableCommand(int id, int doctorId, int hospitalId,
            DateTimeOffset from, DateTimeOffset to, string room)
        {
            Id = id;
            DoctorId = doctorId;
            HospitalId = hospitalId;
            From = from;
            To = to;
            Room = room;
        }
    }
}
