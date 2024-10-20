using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.Entitys
{
    public class Appointment
    {
        public int Id { get; set; }
        public int TimetableId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
