using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Queries
{
    public class GetAppointmentsQuery(int id) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
    }
}