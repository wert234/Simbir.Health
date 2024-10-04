using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Infastructure.Data.DbContexts;

namespace Timetable.Infastructure.Repositories
{
    public class TimetableRepository : IRepository<Domain.Entitys.Timetable, int>
    {

        private readonly TimetableDbContext _timetableDbContext;


        public TimetableRepository(TimetableDbContext timetableDbContext)
        {
            _timetableDbContext = timetableDbContext;
        }


        public async Task AddAsync(Domain.Entitys.Timetable entity)
        {
            await _timetableDbContext.AddAsync(entity);
            await _timetableDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _timetableDbContext.Remove(await GetAsync(id));
            await _timetableDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entitys.Timetable>> GetAllAsync()
        {
            return await _timetableDbContext.Timetables.ToArrayAsync();
        }

        public async Task<Domain.Entitys.Timetable> GetAsync(int id)
        {
            return await _timetableDbContext.Timetables.FirstOrDefaultAsync(timetable => timetable.Id == id);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _timetableDbContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Domain.Entitys.Timetable entity)
        {
            _timetableDbContext.Timetables.Update(entity);
            await _timetableDbContext.SaveChangesAsync();
        }
    }
}
