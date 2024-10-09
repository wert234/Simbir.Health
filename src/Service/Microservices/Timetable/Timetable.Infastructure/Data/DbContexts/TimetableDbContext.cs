using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Domain.Entitys;

namespace Timetable.Infastructure.Data.DbContexts
{
    public class TimetableDbContext : DbContext
    {
        public TimetableDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entitys.Timetable> Timetables { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
