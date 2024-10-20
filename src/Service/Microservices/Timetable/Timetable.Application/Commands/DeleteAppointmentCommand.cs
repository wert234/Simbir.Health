using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Commands
{
    public class DeleteAppointmentCommand(int appointmentId, int userId) : IRequest<IActionResult>
    {
        public int AppointmentId { get; set; } = appointmentId;
        public int UserId { get; set; } = userId;
    }
}
