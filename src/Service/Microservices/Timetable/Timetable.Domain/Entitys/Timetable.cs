using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable.Domain.Entitys
{
    public class Timetable
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Room { get; set; }
    }
}
