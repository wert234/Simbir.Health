using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.Entitys
{
    public class Timetable
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int DoctorId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public string Room { get; set; }
    }
}
