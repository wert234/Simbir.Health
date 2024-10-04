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
        public Guid DoctorId { get; set; }
        public Guid HospitalId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Room { get; set; }
    }
}
