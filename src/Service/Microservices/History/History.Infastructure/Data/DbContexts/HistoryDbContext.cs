using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Infastructure.Data.DbContexts
{
    public class HistoryDbContext : DbContext
    {
        public HistoryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entitys.History> Historys { get; set; }
    }
}
