using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Commands
{
    public class AddAppointmentsCommand(int id, DateTimeOffset time) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
        public DateTimeOffset Time { get; set; } = time;
    }
}
