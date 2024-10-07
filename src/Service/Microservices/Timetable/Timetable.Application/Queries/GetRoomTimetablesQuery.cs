using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Application.Queries
{
    public class GetRoomTimetablesQuery(int id, string room, DateTimeOffset? from, DateTimeOffset? to) : IRequest<IActionResult>
    {
        public int Id { get; set; } = id;
        public string Room { get; set; } = room;
        public DateTimeOffset? From { get; set; } = from;
        public DateTimeOffset? To { get; set; } = to;
    }
}
