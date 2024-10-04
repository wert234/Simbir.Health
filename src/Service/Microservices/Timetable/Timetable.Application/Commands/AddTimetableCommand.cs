using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Commands
{
    public class AddTimetableCommand : IRequest<IActionResult>
    {
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Room { get; set; }
    }
}
