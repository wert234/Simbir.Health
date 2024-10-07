using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Queries
{
    public class GetDoctorTimetableQuery(int id, DateTimeOffset? from, DateTimeOffset? to) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
        public DateTimeOffset? From { get; set; } = from;
        public DateTimeOffset? To { get; set; } = to;
    }
}

