using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Queries
{
    public class GetHospitalTimetableQuery(int id, string? from, string? to) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
        public string? From { get; set; } = from;
        public string? To { get; set; } = to;
    }
}
