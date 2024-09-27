using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Infastructure.Data.DbContexts
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entitys.Hospital> Hospitals { get; set; }
    }
}
