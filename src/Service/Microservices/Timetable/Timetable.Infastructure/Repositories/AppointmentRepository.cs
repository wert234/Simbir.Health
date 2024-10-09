using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Domain.Entitys;
using Timetable.Infastructure.Data.DbContexts;

namespace Timetable.Infastructure.Repositories
{
    public class AppointmentRepository : IRepository<Appointment, int>
    {
        private readonly TimetableDbContext _timetableDbContext;


        public AppointmentRepository(TimetableDbContext timetableDbContext)
        {
            _timetableDbContext = timetableDbContext;
        }
        public async Task AddAsync(Appointment entity)
        {
            await _timetableDbContext.AddAsync(entity);
            await _timetableDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _timetableDbContext.Remove(await GetAsync(id));
            await _timetableDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _timetableDbContext.Appointments
                .ToArrayAsync();
        }

        public async Task<Appointment> GetAsync(int id)
        {
            return await _timetableDbContext.Appointments
                .FirstOrDefaultAsync(timetable => timetable.Id == id);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _timetableDbContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Appointment entity)
        {
            _timetableDbContext.Appointments.Update(entity);
            await _timetableDbContext.SaveChangesAsync();
        }
    }
}
